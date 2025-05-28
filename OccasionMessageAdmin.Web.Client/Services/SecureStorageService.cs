using OccasionMessageAdmin.Shared.Services;
using Microsoft.JSInterop;
using System.Text.Json;

namespace OccasionMessageAdmin.Web.Client.Services;

public class SecureStorageService(IJSRuntime jsRuntime) : ISecureStorageService
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;
    public async Task StoreTokenAsync(string token)
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "authToken", JsonSerializer.Serialize(token));
    }

    public async Task<string> RetrieveTokenAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken");
        return token != null ? JsonSerializer.Deserialize<string>(token) : null;
    }

    public async Task RemoveTokenAsync()
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "authToken");
    }
}
