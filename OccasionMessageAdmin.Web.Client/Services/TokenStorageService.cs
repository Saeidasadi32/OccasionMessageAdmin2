using Microsoft.JSInterop;
using OccasionMessageAdmin.Shared.Interfaces;

namespace OccasionMessageAdmin.Web.Client.Services;

public class TokenStorageService(IJSRuntime js) : ITokenStorageService
{
    public async Task SaveTokensAsync(string token, string refreshToken)
    {
        await js.InvokeVoidAsync("localStorage.setItem", "token", token);
        await js.InvokeVoidAsync("localStorage.setItem", "refreshToken", refreshToken);
    }

    public async Task<(string?, string?)> GetTokensAsync()
    {
        var token = await js.InvokeAsync<string>("localStorage.getItem", "token");
        var refreshToken = await js.InvokeAsync<string>("localStorage.getItem", "refreshToken");
        return (token, refreshToken);
    }

    public async Task RemoveTokensAsync()
    {
        await js.InvokeVoidAsync("localStorage.removeItem", "token");
        await js.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
    }
}

