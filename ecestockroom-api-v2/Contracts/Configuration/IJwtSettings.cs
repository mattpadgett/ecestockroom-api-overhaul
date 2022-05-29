namespace ecestockroom_api_v2.Contracts.Configuration;

public interface IJwtSettings
{
    string Issuer { get; set; }
    string Audience { get; set; }
    string Key { get; set; }
}