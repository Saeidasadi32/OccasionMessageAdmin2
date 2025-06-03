using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OccasionMessageAdmin.Web.Components;
using OccasionMessageAdmin.Web.Config;
using OccasionMessageAdmin.Web.Data;
using OccasionMessageAdmin.Web.Models;
using OccasionMessageAdmin.Web.Services;
using OccasionMessageAdmin.Web.Services.Auth;
using SharedComponents.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var detailedErrors = builder.Configuration.GetValue<bool>("Blazor:DetailedErrors");
builder.Services.AddServerSideBlazor(options =>
{
    options.DetailedErrors = detailedErrors;
});

builder.Services.AddControllers();

builder.Services.AddDbContext<OccasionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OccasionDb")));

// ConnectionString
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Identity")));

// Identity Config
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


// JWT Configuration Validation
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
if (jwtSettings == null || !jwtSettings.IsValid())
{
    throw new InvalidOperationException("Invalid JWT settings. Please check 'Jwt' section in appsettings.");
}
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ClockSkew = TimeSpan.Zero
        };
        options.RequireHttpsMetadata = false; // فقط در dev
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseAntiforgery();
app.MapControllers();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(OccasionMessageAdmin.Shared._Imports).Assembly,
        typeof(OccasionMessageAdmin.Web.Client._Imports).Assembly);

app.Run();
