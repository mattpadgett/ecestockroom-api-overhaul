using ecestockroom_api_v2.Contracts.Requests.Auth;
using ecestockroom_api_v2.Contracts.Responses.Auth;
using ecestockroom_api_v2.Services.Auth;
using FastEndpoints;

namespace ecestockroom_api_v2.Endpoints.Auth;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly AuthService _authService;
    
    public LoginEndpoint(AuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/api/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        await SendOkAsync(new ()
        {
            
        }, ct);
    }
}