using OccasionMessageAdmin.Shared.Models.Auth;
using OccasionMessageAdmin.Web.Models;
namespace OccasionMessageAdmin.Web.Services.Auth;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<string> GenerateJwtToken(ApplicationUser user);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
}
