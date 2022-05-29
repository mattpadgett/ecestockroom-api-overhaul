namespace ecestockroom_api_v2.Contracts.Requests.Auth;

public class RefreshRequest
{
    public string AuthorizationToken { get; set; }
    public string RefreshToken { get; set; }
}