using OccasionMessageAdmin.Shared.Interfaces;

namespace OccasionMessageAdmin.Services;

public class TokenStorageService : ITokenStorageService
{
    public Task SaveTokensAsync(string token, string refreshToken)
    {
        SecureStorage.Default.SetAsync("token", token);
        SecureStorage.Default.SetAsync("refreshToken", refreshToken);
        return Task.CompletedTask;
    }

    public async Task<(string?, string?)> GetTokensAsync()
    {
        var token = await SecureStorage.Default.GetAsync("token");
        var refreshToken = await SecureStorage.Default.GetAsync("refreshToken");
        return (token, refreshToken);
    }

    public Task RemoveTokensAsync()
    {
        SecureStorage.Default.Remove("token");
        SecureStorage.Default.Remove("refreshToken");
        return Task.CompletedTask;
    }
}
