using OccasionMessageAdmin.Web.Models;

namespace OccasionMessageAdmin.Web.Services;

public interface IRefreshTokenService
{
    Task<RefreshToken> GenerateAsync(ApplicationUser user);
    Task<bool> ValidateAsync(string token);
    Task RevokeAsync(string token);
}
