namespace OccasionMessageAdmin.Shared.Interfaces;

public interface ISessionStorageService
{
    ValueTask SetItemAsync<T>(string key, T value);
    ValueTask<T> GetItemAsync<T>(string key);
    ValueTask RemoveItemAsync(string key);
    ValueTask<bool> ContainsKeyAsync(string key);
    ValueTask ClearAsync();
}
