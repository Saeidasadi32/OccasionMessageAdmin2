using OccasionMessageAdmin.Shared.Models.Auth;
using OccasionMessageAdmin.Shared.Services;
using System.Net.Http.Json;

namespace OccasionMessageAdmin.Web.Client.Services;

public class AuthService(HttpClient httpClient) : IAuthService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
        return await response.Content.ReadFromJsonAsync<AuthResponse>()
            ?? new AuthResponse { IsSuccess = false, Errors = ["Unexpected error."] };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
        return await response.Content.ReadFromJsonAsync<AuthResponse>()
            ?? new AuthResponse { IsSuccess = false, Errors = ["Unexpected error."] };
    }

    public Task LogoutAsync()
    {
        // WebAssembly معمولاً token رو در localStorage نگه می‌داره
        // اینجا مثلاً localStorage.Clear() یا مشابه می‌تونه باشه
        return Task.CompletedTask;
    }

    public Task<string> GetCurrentTokenAsync()
    {
        // از localStorage یا ProtectedLocalStorage بخونه
        return Task.FromResult(string.Empty);
    }
}