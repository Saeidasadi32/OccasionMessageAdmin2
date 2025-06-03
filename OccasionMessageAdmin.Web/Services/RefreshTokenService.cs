using Microsoft.EntityFrameworkCore;
using OccasionMessageAdmin.Web.Data;
using OccasionMessageAdmin.Web.Models;

namespace OccasionMessageAdmin.Web.Services;

public class RefreshTokenService(ApplicationDbContext db) : IRefreshTokenService
{
    private readonly ApplicationDbContext _db = db;

    public async Task<RefreshToken> GenerateAsync(ApplicationUser user)
    {
        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString("N"),
            Expiration = DateTime.UtcNow.AddDays(10),
            UserId = user.Id
        };

        _db.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<bool> ValidateAsync(string token)
    {
        var existing = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token && !x.IsRevoked);
        return existing != null && existing.Expiration >= DateTime.UtcNow;
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
}
