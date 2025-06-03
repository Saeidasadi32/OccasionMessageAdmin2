using System.Security.Cryptography;
using System.Text;

namespace OccasionMessageAdmin.Web.Helper;

public static class AesEncryptionHelper
{
    public static string Encrypt(string plainText, string key)
    {
        using var aes = Aes.Create();
        var keyBytes = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
        aes.Key = keyBytes;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        ms.Write(aes.IV, 0, aes.IV.Length);
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var sw = new StreamWriter(cs);
        sw.Write(plainText);
        sw.Close();
        return Convert.ToBase64String(ms.ToArray());
    }
}