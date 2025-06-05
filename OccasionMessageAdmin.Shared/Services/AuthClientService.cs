using OccasionMessageAdmin.Shared.Interfaces;
using OccasionMessageAdmin.Shared.Models.Auth;
using System.Net.Http.Json;

namespace OccasionMessageAdmin.Shared.Services;

public class AuthClientService(HttpClient client, ITokenStorageService storage)
{
    private readonly HttpClient _client = client;
    private readonly ITokenStorageService _storage = storage;

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var response = await _client.PostAsJsonAsync("api/auth/login", request);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

        if (result?.IsSuccess == true)
        {
            await storage.SaveTokensAsync(result.Token!, result.RefreshToken!);
        }

        return result!;
    }

    public async Task LogoutAsync()
    {
        await storage.RemoveTokensAsync();
    }

    public async Task<AuthResponse?> RefreshAsync()
    {
        var (_, refreshToken) = await storage.GetTokensAsync();
        if (string.IsNullOrWhiteSpace(refreshToken)) return null;

        var response = await _client.PostAsJsonAsync("api/auth/refresh", new { RefreshToken = refreshToken });
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

        if (result?.IsSuccess == true)
        {
            await storage.SaveTokensAsync(result.Token!, result.RefreshToken!);
        }

        return result;
    }
}
