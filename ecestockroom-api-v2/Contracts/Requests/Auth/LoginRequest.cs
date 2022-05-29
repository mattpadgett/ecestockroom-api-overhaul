namespace ecestockroom_api_v2.Contracts.Requests.Auth;

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}