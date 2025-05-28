namespace OccasionMessageAdmin.Shared.Services;

public interface INavigationService
{
    Task NavigateToAsync(string route);
    Task GoBackAsync();
}