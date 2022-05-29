using ecestockroom_api_v2.Contracts.Requests.Auth;
using ecestockroom_api_v2.Contracts.Responses.Auth;
using ecestockroom_api_v2.Domain;
using ecestockroom_api_v2.Helpers.Security;

namespace ecestockroom_api_v2.Endpoints.Auth;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly JwtHelpers _jwtHelpers;
    private readonly TokenFamilyService _tokenFamilyService;
    private readonly UserService _userService;
    private readonly RoleService _roleService;
    private readonly PermissionService _permissionService;
    
    public LoginEndpoint(JwtHelpers jwtHelpers, TokenFamilyService tokenFamilyService, UserService userService, RoleService roleService, PermissionService permissionService)
    {
        _jwtHelpers = jwtHelpers;
        _tokenFamilyService = tokenFamilyService;
        _userService = userService;
        _roleService = roleService;
        _permissionService = permissionService;
    }

    public override void Configure()
    {
        Post("/api/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        // Load user
        User? user = await _userService.GetUser(req.Email);

        if (user == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        // Check password
        if (!PasswordHelpers.IsPasswordValid(req.Password, user.PasswordHash))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        
        // Check roles for login access
        bool isLoginGranted = false;
        
        var roles = await _roleService.Get(); // Preload all the roles so we aren't querying a bunch if we don't really need to be
        var loginPermission = await _permissionService.Get("login"); // Preload login permission
        
        if (loginPermission == null)
        {
            throw new NullReferenceException("Somebody deleted or modified the login permission...");
        }

        foreach (var userRoleId in user.RoleIds)
        {
            var role = roles.First(x => x.RoleId == userRoleId);

            if (role.PermissionIds.ToList().Contains(loginPermission.PermissionId))
            {
                isLoginGranted = true;
            }
        }

        // Check permissions for login access
        if (user.PermissionIds.ToList().Contains(loginPermission.PermissionId))
        {
            isLoginGranted = true;
        }

        if (!isLoginGranted)
        {
            await SendForbiddenAsync(ct);
            return;
        }
        
        // Generate and store authorization and refresh tokens
        string authorizationToken = _jwtHelpers.GenerateAuthorizationToken(user, await _permissionService.Get(user.PermissionIds.ToList()));
        string refreshToken = _jwtHelpers.GenerateRefreshToken(user);

        await _tokenFamilyService.Create(new()
        {
            AuthorizationToken = authorizationToken,
            RefreshToken = refreshToken,
            CreationUtc = DateTime.UtcNow,
            CreationReason = "Login",
            UserId = user.UserId,
            ValidFlag = true
        });
        
        // Send tokens back
        await SendOkAsync(new ()
        {
            AuthorizationToken = authorizationToken,
            RefreshToken = refreshToken
        }, ct);
    }
}