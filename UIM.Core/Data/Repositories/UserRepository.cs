namespace UIM.Core.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UimContext _context;

    public UserRepository(UimContext context) => _context = context;

    public async Task<bool> AddRefreshTokenAsync(AppUser user, RefreshToken refreshToken)
    {
        user.RefreshTokens.Add(refreshToken);
        _context.Update(user);
        var created = await _context.SaveChangesAsync();
        return created > 0;
    }

    public AppUser GetByRefreshToken(string? token)
    {
        var user = _context.Users.First(u =>
            u.Id == u.RefreshTokens.First(t => t.Token == token).UserId);
        return user;
    }

    public RefreshToken GetRefreshToken(string token)
    {
        var user = GetByRefreshToken(token);
        var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
        return refreshToken;
    }

    public async Task<bool> RemoveOutdatedRefreshTokensAsync(AppUser user)
    {
        user.RefreshTokens.RemoveAll(t => !t.IsActive || t.ExpiredDate <= DateTime.Now);
        var removed = await _context.SaveChangesAsync();
        return removed > 0;
    }

    public async Task<bool> RevokeRefreshTokenAsync(RefreshToken token, string? reason = null,
        string? replacedByToken = null)
    {
        var user = await _context.Users.FindAsync(token.UserId);
        if (user == null) return false;

        RevokeToken(token, reason, replacedByToken);
        _context.Update(user);
        var revoked = await _context.SaveChangesAsync();
        return revoked > 0;
    }

    public async Task<bool> RevokeRefreshTokenDescendantsAsync(RefreshToken token,
        AppUser user, string reason)
    {
        // recursively traverse the refresh token chain and ensure all descendants are revoked
        var revoked = 0;
        if (!string.IsNullOrEmpty(token.ReplacedByToken))
        {
            var childToken = user.RefreshTokens.SingleOrDefault(x =>
                x.Token == token.ReplacedByToken);

            if (childToken == null || childToken.IsActive) return false;

            if (childToken.IsActive)
                RevokeToken(childToken, reason);
            else
                await RevokeRefreshTokenDescendantsAsync(childToken, user, reason);

            revoked = await _context.SaveChangesAsync();
        }
        return revoked > 0;
    }

    private static void RevokeToken(RefreshToken token, string? reason = null,
        string? replacedByToken = null)
    {
        token.RevokedDate = DateTime.Now;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }
}