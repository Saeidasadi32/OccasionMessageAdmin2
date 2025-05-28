using OccasionMessageAdmin.Shared.Models.Auth;

namespace OccasionMessageAdmin.Shared.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
    Task<string> GetCurrentTokenAsync();
}
