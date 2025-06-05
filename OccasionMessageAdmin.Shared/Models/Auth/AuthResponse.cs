namespace OccasionMessageAdmin.Shared.Models.Auth;

public class AuthResponse
{
    public bool IsSuccess { get; set; }
    public string? Token { get; set; }
    public DateTime? ExpireAt { get; set; }
    public string? UserId { get; set; }
    public string? RefreshToken { get; set; }
    public IEnumerable<string>? Roles { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
