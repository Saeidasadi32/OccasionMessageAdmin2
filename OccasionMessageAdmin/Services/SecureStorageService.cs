using OccasionMessageAdmin.Shared.Interfaces;

namespace FamilyBook.Services;

public class SecureStorageService : ISecureStorageService
{
    public async Task StoreTokenAsync(string token)
    {
        await SecureStorage.SetAsync("authToken", token);        
    }

    public async Task<string> RetrieveTokenAsync()
    {
        return await SecureStorage.GetAsync("authToken");
    }

    public async Task RemoveTokenAsync()
    {
        SecureStorage.Remove("authToken");
    }
}
