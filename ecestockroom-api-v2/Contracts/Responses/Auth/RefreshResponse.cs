namespace ecestockroom_api_v2.Contracts.Responses.Auth;

public class RefreshResponse
{
    public string AuthorizationToken { get; set; }
    public string RefreshToken { get; set; }
}