using OccasionMessageAdmin.Shared.Models.Auth;
using OccasionMessageAdmin.Shared.Services;
using System.Net.Http.Json;

namespace OccasionMessageAdmin.Services;

public class AuthService(HttpClient httpClient) : IAuthService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>()
                     ?? new AuthResponse { IsSuccess = false, Errors = ["Unexpected error."] };

        if (result.IsSuccess && !string.IsNullOrEmpty(result.Token))
        {
            await SecureStorage.SetAsync("jwt_token", result.Token);
        }

        return result;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
        return await response.Content.ReadFromJsonAsync<AuthResponse>()
            ?? new AuthResponse { IsSuccess = false, Errors = ["Unexpected error."] };
    }

    public async Task LogoutAsync()
    {
        SecureStorage.Remove("jwt_token");
        await Task.CompletedTask;
    }

    public async Task<string> GetCurrentTokenAsync()
    {
        return await SecureStorage.GetAsync("jwt_token") ?? string.Empty;
    }
}