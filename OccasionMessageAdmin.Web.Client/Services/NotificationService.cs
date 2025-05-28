using Microsoft.JSInterop;
using OccasionMessageAdmin.Shared.Services;

namespace OccasionMessageAdmin.Web.Client.Services;

public class NotificationService : INotificationService
{
    private readonly IJSRuntime _js;

    public NotificationService(IJSRuntime js)
    {
        _js = js;
    }

    public Task ShowMessageAsync(string message, string title = "Info")
        => _js.InvokeVoidAsync("alert", $"{title}: {message}").AsTask();

    public Task ShowErrorAsync(string message, string title = "Error")
        => ShowMessageAsync(message, title);

    public Task ShowSuccessAsync(string message, string title = "Success")
        => ShowMessageAsync(message, title);
}