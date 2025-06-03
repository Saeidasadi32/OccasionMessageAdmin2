using Microsoft.Extensions.Logging;
using OccasionMessageAdmin.Services;
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

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
