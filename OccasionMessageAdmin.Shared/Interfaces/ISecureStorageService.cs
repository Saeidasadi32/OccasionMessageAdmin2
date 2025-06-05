namespace OccasionMessageAdmin.Shared.Interfaces;

public interface ISecureStorageService
{
    Task StoreTokenAsync(string token);
    Task<string> RetrieveTokenAsync();
    Task RemoveTokenAsync();
}