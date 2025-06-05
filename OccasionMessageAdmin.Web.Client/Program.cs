using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OccasionMessageAdmin.Shared.Interfaces;
using OccasionMessageAdmin.Shared.Services;
using OccasionMessageAdmin.Web.Client.Services;
using SharedComponents.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// HttpClient اصلی پروژه
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<AuthHttpMessageHandler>();
builder.Services.AddScoped<AuthClientService>();
builder.Services.AddScoped<ITokenStorageService, TokenStorageService>();
builder.Services.AddHttpClient("AuthHttpClient")
    .AddHttpMessageHandler<AuthHttpMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthHttpClient"));

// SharedComponents
builder.Services.AddSharedComponents();

// سایر سرویس‌ها
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INavigationService, NavigationService>();
builder.Services.AddScoped<INotificationService, NotificationService>();


await builder.Build().RunAsync();