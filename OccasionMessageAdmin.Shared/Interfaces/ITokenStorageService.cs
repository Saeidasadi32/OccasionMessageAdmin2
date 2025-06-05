
namespace OccasionMessageAdmin.Shared.Interfaces;

public interface ITokenStorageService
{
    Task SaveTokensAsync(string token, string refreshToken);
    Task<(string? Token, string? RefreshToken)> GetTokensAsync();
    Task RemoveTokensAsync();
}