using Microsoft.AspNetCore.Components;
using OccasionMessageAdmin.Shared.Services;

namespace OccasionMessageAdmin.Web.Client.Services;

public class NavigationService(NavigationManager navigationManager) : INavigationService
{
    private readonly NavigationManager _navigationManager = navigationManager;

    public Task NavigateToAsync(string route)
    {
        _navigationManager.NavigateTo(route);
        return Task.CompletedTask;
    }

    public Task GoBackAsync()
    {
        // WebAssembly به صورت native تاریخچه ندارد، ولی می‌شود JSInterop نوشت
        // فعلاً می‌گذاریم navigate به root
        _navigationManager.NavigateTo("/");
        return Task.CompletedTask;
    }
}
