
using FamilyBook.Services;
using OccasionMessageAdmin.Shared.Interfaces;

namespace OccasionMessageAdmin.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthServiceService(this IServiceCollection services)
    {
        services.AddSingleton<IAuthService, AuthService>();
        return services;
    }

    public static IServiceCollection AddNavigationService(this IServiceCollection services)
    {
        services.AddSingleton<INavigationService, NavigationService>();
        return services;
    }

    public static IServiceCollection AddNotificationService(this IServiceCollection services)
    {
        services.AddSingleton<INotificationService, NotificationService>();
        return services;
    }

    public static IServiceCollection AddLocalStorageService(this IServiceCollection services)
    {
        services.AddSingleton<ILocalStorageService, MauiLocalStorageService>();
        return services;
    }

    public static IServiceCollection AddSessionStorageService(this IServiceCollection services)
    {
        services.AddSingleton<ISessionStorageService, MauiSessionStorageService>();
        return services;
    }
    public static IServiceCollection AddHttpClientService(this IServiceCollection services)
    {
        var BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7174/" : "https://localhost:7174");

        services.AddSingleton<IHttpsClientHandlerService, HttpsClientHandlerService>();

        // Register HttpClient with the custom handler
        services.AddSingleton(sp =>
        {
            var handlerService = sp.GetRequiredService<IHttpsClientHandlerService>();
            var handler = handlerService.GetPlatformMessageHandler();
            if (handler != null)
                return new HttpClient(handler)
                {
                    BaseAddress = BaseAddress
                };
            else
                return new HttpClient()
                {
                    BaseAddress = BaseAddress
                };
        });

        return services;
    }
}