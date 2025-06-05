using System.Security.Cryptography;

namespace OccasionMessageAdmin.Web.Helper;

public static class TokenHelper
{
    public static string GenerateSecureToken(int length = 64)
    {
        var randomNumber = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
