using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ecestockroom_api_v2.Contracts.Configuration;
using ecestockroom_api_v2.Domain;
using Microsoft.IdentityModel.Tokens;

namespace ecestockroom_api_v2.Helpers.Security;

public class JwtHelpers
{
    private readonly IJwtSettings _jwtSettings;

    public JwtHelpers(IJwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public string GenerateAuthorizationToken(User user, List<Permission> permissions)
    {
        return JWTBearer.CreateToken(
            signingKey: _jwtSettings.Key,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expireAt: DateTime.UtcNow.AddDays(15),
            permissions: permissions.Select(x => x.Key),
            claims: new[] { new Claim("userId", user.UserId.ToString()) }
        );
    }

    public string GenerateRefreshToken(User user)
    {
        return JWTBearer.CreateToken(
            signingKey: _jwtSettings.Key,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expireAt: DateTime.UtcNow.AddMinutes(15),
            claims: new[]
            {
                new Claim("userId", user.UserId.ToString()),
            }
        );
    }

    public bool IsValid(string token)
    {
        try
        {
            new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key)),
                ClockSkew = TimeSpan.Zero
            }, out _);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public static JwtSecurityToken ReadToken(string token)
    {
        return new JwtSecurityTokenHandler().ReadJwtToken(token);
    }
}