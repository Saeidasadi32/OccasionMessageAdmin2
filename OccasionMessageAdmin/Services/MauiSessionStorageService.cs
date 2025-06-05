using OccasionMessageAdmin.Shared.Interfaces;
using System.Text.Json;

namespace FamilyBook.Services;

public class MauiSessionStorageService : ISessionStorageService
{
    public async ValueTask SetItemAsync<T>(string key, T value)
    {
        await Task.Run(() => Preferences.Set(key, JsonSerializer.Serialize(value)));
    }

    public async ValueTask<T> GetItemAsync<T>(string key)
    {
        var value = Preferences.Get(key, null);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }

    public async ValueTask RemoveItemAsync(string key)
    {
        await Task.Run(() => Preferences.Remove(key));
    }

    public async ValueTask<bool> ContainsKeyAsync(string key)
    {
        return await Task.Run(() => Preferences.ContainsKey(key));
    }

    public async ValueTask ClearAsync()
    {
        await Task.Run(() => Preferences.Clear());
    }
}
