namespace ecestockroom_api_v2.Contracts.Responses.Auth;

public class LoginResponse
{
    public string AuthorizationToken { get; set; } = String.Empty;
    public string RefreshToken { get; set; } = String.Empty;
}