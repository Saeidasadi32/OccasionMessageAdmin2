using OccasionMessageAdmin.Shared.Interfaces;

namespace OccasionMessageAdmin.Services;

public class NotificationService : INotificationService
{
    public Task ShowMessageAsync(string message, string title = "Info")
    {
        Shell.Current.DisplayAlert(title, message, "OK");
        return Task.CompletedTask;
    }

    public Task ShowErrorAsync(string message, string title = "Error")
        => ShowMessageAsync(message, title);

    public Task ShowSuccessAsync(string message, string title = "Success")
        => ShowMessageAsync(message, title);
}
