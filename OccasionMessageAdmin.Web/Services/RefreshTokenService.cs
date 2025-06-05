using Microsoft.EntityFrameworkCore;
using OccasionMessageAdmin.Web.Data;
using OccasionMessageAdmin.Web.Helper;
using OccasionMessageAdmin.Web.Models;

namespace OccasionMessageAdmin.Web.Services;

public class RefreshTokenService(ApplicationDbContext db) : IRefreshTokenService
{
    private readonly ApplicationDbContext _db = db;

    public async Task<RefreshToken> GenerateAsync(ApplicationUser user)
    {
        var refreshToken = new RefreshToken
        {
            Token = TokenHelper.GenerateSecureToken(),
            Expiration = DateTime.UtcNow.AddDays(10),
            UserId = user.Id
        };

        _db.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<RefreshToken> CreateAsync(string userId)
    {
        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Token = TokenHelper.GenerateSecureToken(),
            Expiration = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        _db.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync();
        return refreshToken;
    }

    public async Task AddAsync(RefreshToken token)
    {
        _db.RefreshTokens.Add(token);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> ValidateAsync(string token)
    {
        return await _db.RefreshTokens.AnyAsync(x => x.Token == token && !x.IsRevoked && x.Expiration >= DateTime.UtcNow);
    }

    public async Task RevokeAsync(string token)
    {
        var existing = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        if (existing != null)
        {
            existing.IsRevoked = true;
            await _db.SaveChangesAsync();
        }
    }
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
    }
}
