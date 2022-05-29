namespace ecestockroom_api_v2.Contracts.Configuration;

public class JwtSettings : IJwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
}