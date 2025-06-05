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

public class AuthService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings, IRefreshTokenService refreshTokenService) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly JwtSettings _jwtSettings = jwtSettings;
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new AuthResponse { IsSuccess = false, Errors = result.Errors.Select(e => e.Description) };
        }
        var token = await GenerateJwtToken(user);
        var refreshToken = await _refreshTokenService.CreateAsync(user.Id);

        return new AuthResponse
        {
            IsSuccess = true,
            Token = token,
            RefreshToken = refreshToken.Token
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return new AuthResponse { IsSuccess = false, Errors = ["Email or Password is incorrect"] };
        }
        
        var token = await GenerateJwtToken(user);
        var refreshToken = await _refreshTokenService.CreateAsync(user.Id);

        return new AuthResponse
        {
            IsSuccess = true,
            Token = token,
            RefreshToken = refreshToken.Token
        };
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
        if (_jwtSettings.Key.Length != 32)
            throw new InvalidOperationException("Encryption key must be 32 characters long.");

        var encryptedToken = AesEncryptionHelper.Encrypt(tokenString, _jwtSettings.Key);
        return encryptedToken;
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _refreshTokenService.GetByTokenAsync(refreshToken);

        if (storedToken == null || storedToken.IsRevoked || storedToken.Expiration < DateTime.UtcNow)
        {
            return new AuthResponse { IsSuccess = false, Errors = ["Refresh token is invalid or expired."] };
        }

        var user = await _userManager.FindByIdAsync(storedToken.UserId);
        if (user == null)
        {
            return new AuthResponse { IsSuccess = false, Errors = ["User not found."] };
        }

        // Revoke old refresh token
        await _refreshTokenService.RevokeAsync(storedToken.Token);

        // Generate new JWT
        var jwtToken = await GenerateJwtToken(user);

        // Generate new RefreshToken
        var newRefreshToken = await _refreshTokenService.CreateAsync(user.Id);

        return new AuthResponse
        {
            IsSuccess = true,
            Token = jwtToken,
            RefreshToken = newRefreshToken.Token
        };
    }
}
