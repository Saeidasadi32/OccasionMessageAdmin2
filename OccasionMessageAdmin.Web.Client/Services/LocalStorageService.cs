using Microsoft.JSInterop;
using System.Text.Json;
using OccasionMessageAdmin.Shared.Interfaces;

namespace OccasionMessageAdmin.Web.Client.Services;
public class WebAssemblyLocalStorageService(IJSRuntime jsRuntime) : ILocalStorageService
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;
    public async ValueTask SetItemAsync<T>(string key, T value)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
    }

    public async ValueTask<T> GetItemAsync<T>(string key)
    {
        var value = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }

    public async ValueTask RemoveItemAsync(string key)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }

    public async ValueTask<bool> ContainsKeyAsync(string key)
    {
        var value = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        return value != null;
    }

    public async ValueTask ClearAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.clear");
    }
}

