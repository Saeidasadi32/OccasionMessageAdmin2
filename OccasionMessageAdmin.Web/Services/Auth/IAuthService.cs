using OccasionMessageAdmin.Shared.Models.Auth;
namespace OccasionMessageAdmin.Web.Services.Auth;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
