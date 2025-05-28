namespace OccasionMessageAdmin.Shared.Services;

public interface ISecureStorageService
{
    Task StoreTokenAsync(string token);
    Task<string> RetrieveTokenAsync();
    Task RemoveTokenAsync();
}