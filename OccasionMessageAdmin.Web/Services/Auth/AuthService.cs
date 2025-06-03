using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OccasionMessageAdmin.Shared.Models.Auth;
using OccasionMessageAdmin.Web.Config;
using OccasionMessageAdmin.Web.Helper;
using OccasionMessageAdmin.Web.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OccasionMessageAdmin.Web.Services.Auth;

public class AuthService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly JwtSettings _jwtSettings = jwtSettings;

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var user = new ApplicationUser { Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new AuthResponse { IsSuccess = false, Errors = result.Errors.Select(e => e.Description) };
        }

        var token =await GenerateJwtToken(user);
        return new AuthResponse { IsSuccess = true, Token = token };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return new AuthResponse { IsSuccess = false, Errors = ["Email or Password is incorrect"] };
        }

        var token =await GenerateJwtToken(user);
        return new AuthResponse { IsSuccess = true, Token = token };
    }

    public async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Email),
            new("FirstName", user.FirstName ?? ""),
            new("LastName", user.LastName ?? ""),
            new("LanguageCode", user.LanguageCode ?? ""),
            new("RefId", user.RefId.ToString()),
            new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: now.AddMinutes(_jwtSettings.ExpireMinutes),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        var encryptedToken = AesEncryptionHelper.Encrypt(tokenString, _jwtSettings.Key);
        return encryptedToken;
    }
}
