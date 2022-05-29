using System.Security.Cryptography;

namespace ecestockroom_api_v2.Helpers.Security;

public class PasswordHelpers
{
    private const int PasswordSaltSize = 16;
    private const int PasswordHashSize = 48;
    private const int PasswordHashIterations = 100000;
    
    public static string EncryptPassword(string password)
    {
        byte[] salt;
        RandomNumberGenerator.Fill(salt = new byte[PasswordSaltSize]);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, PasswordHashIterations);
        var hash = pbkdf2.GetBytes(PasswordHashSize);

        var hashBytes = new byte[PasswordSaltSize + PasswordHashSize];
        Array.Copy(salt, 0, hashBytes, 0, PasswordSaltSize);
        Array.Copy(hash, 0, hashBytes, PasswordSaltSize, PasswordHashSize);

        var base64Hash = Convert.ToBase64String(hashBytes);

        return $"$STOCKROOM$V1${PasswordHashIterations}${base64Hash}";
    }
    
    public static bool IsPasswordValid(string password, string hashedPassword)
    {
        var splitHashString = hashedPassword.Replace("$STOCKROOM$V1$", "").Split('$');
        var iterations = int.Parse(splitHashString[0]);
        var base64Hash = splitHashString[1];

        var hashBytes = Convert.FromBase64String(base64Hash);

        var salt = new byte[PasswordSaltSize];
        Array.Copy(hashBytes, 0, salt, 0, PasswordSaltSize);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
        byte[] hash = pbkdf2.GetBytes(PasswordHashSize);

        for (var i = 0; i < PasswordHashSize; i++)
        {
            if (hashBytes[i + PasswordSaltSize] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}