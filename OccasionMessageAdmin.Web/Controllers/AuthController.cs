using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OccasionMessageAdmin.Shared.Models.Auth;
using OccasionMessageAdmin.Web.Config;
using OccasionMessageAdmin.Web.Data;
using OccasionMessageAdmin.Web.DTOs;
using OccasionMessageAdmin.Web.Models;
using OccasionMessageAdmin.Web.Services;
using OccasionMessageAdmin.Web.Services.Auth;

namespace OccasionMessageAdmin.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, IRefreshTokenService refreshTokenService, ApplicationDbContext db, UserManager<ApplicationUser> userManager, JwtSettings jwtSettings) : ControllerBase
{
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
    private readonly ApplicationDbContext _db = db;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly JwtSettings _jwtSettings = jwtSettings;
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return result.IsSuccess ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest model)
    {
        var tokenIsValid = await _refreshTokenService.ValidateAsync(model.RefreshToken);
        if (!tokenIsValid)
            return Unauthorized("Invalid or expired refresh token");

        var existingToken = await _db.RefreshTokens.Include(x => x.UserId).FirstOrDefaultAsync(x => x.Token == model.RefreshToken);
        if (existingToken == null)
            return Unauthorized("Invalid refresh token");

        var user = await _db.Users.FindAsync(existingToken.UserId);

        var newJwt = await authService.GenerateJwtToken(user!);
        var newRefresh = await _refreshTokenService.GenerateAsync(user!);

        await _refreshTokenService.RevokeAsync(model.RefreshToken);

        return Ok(new { Token = newJwt, RefreshToken = newRefresh.Token });
    }
}
