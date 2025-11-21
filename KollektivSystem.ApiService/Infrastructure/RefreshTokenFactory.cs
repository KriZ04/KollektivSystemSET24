using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Infrastructure;

public static class RefreshTokenFactory
{
    public static (string Plain, RefreshToken Entity) CreateForUser(User user, DateTime? now = null)
    {
        now ??= DateTime.UtcNow;
        var plain = Guid.NewGuid().ToString("N");
        var hash = TokenHash.Hash(plain);

        var entity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = hash,
            CreatedAt = now.Value,
            ExpiresAt = now.Value.AddDays(7),
            Revoked = false
        };

        return (plain, entity);
    }
}
