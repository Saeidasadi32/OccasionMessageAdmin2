namespace OccasionMessageAdmin.Shared.Services;

public interface INotificationService
{
    Task ShowMessageAsync(string message, string title = "Info");
    Task ShowErrorAsync(string message, string title = "Error");
    Task ShowSuccessAsync(string message, string title = "Success");
}
