using Microsoft.Extensions.Logging;
using OccasionMessageAdmin.Services;
using OccasionMessageAdmin.Shared.Interfaces;
using OccasionMessageAdmin.Shared.Services;
using SharedComponents.Extensions;
using SharedComponents.Services;
using SharedFormComponents.Models;
using System.Text.Json;

namespace OccasionMessageAdmin
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddHttpClientService();
            builder.Services.AddAuthServiceService();
            builder.Services.AddNavigationService();
            builder.Services.AddNotificationService();
            builder.Services.AddSharedComponents();
            builder.Services.AddSingleton<ITokenStorageService, TokenStorageService>();
            builder.Services.AddSingleton<AuthClientService>();
            builder.Services.AddSingleton<AuthHttpMessageHandler>();
            builder.Services.AddHttpClient("AuthHttpClient")
                .AddHttpMessageHandler<AuthHttpMessageHandler>();

            builder.Services.AddSingleton(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthHttpClient"));


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
