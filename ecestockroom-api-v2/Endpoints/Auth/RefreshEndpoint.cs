using ecestockroom_api_v2.Contracts.Requests.Auth;
using ecestockroom_api_v2.Contracts.Responses.Auth;
using ecestockroom_api_v2.Helpers.Security;
using FluentValidation;

namespace ecestockroom_api_v2.Endpoints.Auth;

public class RefreshEndpoint : Endpoint<RefreshRequest, RefreshResponse>
{
    private readonly JwtHelpers _jwtHelpers;
    private readonly TokenFamilyService _tokenFamilyService;
    private readonly UserService _userService;
    private readonly PermissionService _permissionService;
    
    public RefreshEndpoint(TokenFamilyService tokenFamilyService, UserService userService, JwtHelpers jwtHelpers, PermissionService permissionService)
    {
        _tokenFamilyService = tokenFamilyService;
        _userService = userService;
        _jwtHelpers = jwtHelpers;
        _permissionService = permissionService;
    }

    public override void Configure()
    {
        Post("/api/auth/refresh");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RefreshRequest req, CancellationToken ct)
    {
        var tokenFamily = await _tokenFamilyService.GetByRefreshToken(req.RefreshToken);

        if (tokenFamily is null)
        {
            ThrowError("token family not found");
            return;
        }
        
        // Check if auth token matches
        if (!tokenFamily.AuthorizationToken.Equals(req.AuthorizationToken))
        {
            ThrowError("authorization token does not coincide with refresh token");
            return;
        }
        
        // Check if token family is valid
        if (!tokenFamily.ValidFlag)
        {
            await _tokenFamilyService.InvalidateByUserId(tokenFamily.UserId);
            await SendUnauthorizedAsync(ct);
            return;
        }
        
        if (!_jwtHelpers.IsValid(req.RefreshToken))
        {
            ThrowError("token validation failed");
            return;
        }
        
        await _tokenFamilyService.Invalidate(tokenFamily.TokenFamilyId);

        var user = await _userService.GetUser(tokenFamily.UserId);

        if (user is null)
        {
            ThrowError("user could not be loaded");
            return;
        }
        
        // TODO: Make a function to extract role permission ids as well so everything is just translated to permission ids
        string authorizationToken = _jwtHelpers.GenerateAuthorizationToken(user, await _permissionService.Get(user.PermissionIds.ToList()));
        string refreshToken = _jwtHelpers.GenerateRefreshToken(user);
        
        await _tokenFamilyService.Create(new()
        {
            AuthorizationToken = authorizationToken,
            RefreshToken = refreshToken,
            CreationUtc = DateTime.UtcNow,
            CreationReason = "Refresh",
            UserId = user.UserId,
            ValidFlag = true
        });

        await SendOkAsync(new ()
        {
            AuthorizationToken = authorizationToken,
            RefreshToken = refreshToken
        }, ct);
    }
}