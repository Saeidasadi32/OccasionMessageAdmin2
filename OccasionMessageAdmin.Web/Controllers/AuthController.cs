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
        var result = await _authService.RefreshTokenAsync(model.RefreshToken);
        return result.IsSuccess ? Ok(result) : Unauthorized(result.Errors);
    }

}
