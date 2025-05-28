using System.ComponentModel.DataAnnotations;

namespace OccasionMessageAdmin.Web.Config;

public class JwtSettings
{
    [Required, MinLength(32, ErrorMessage = "Key must be at least 32 characters long.")]
    public string Key { get; set; } = string.Empty;

    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "ExpireMinutes must be greater than 0.")]
    public int ExpireMinutes { get; set; }
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Issuer) &&
               !string.IsNullOrWhiteSpace(Audience) &&
               !string.IsNullOrWhiteSpace(Key) &&
               ExpireMinutes > 0 &&
               Key.Length >= 32;
    }
}
