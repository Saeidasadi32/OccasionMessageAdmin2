namespace OccasionMessageAdmin.Web.Models;

public class RefreshToken
{
    public long Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public bool IsRevoked { get; set; }
}
