namespace UIM.Core.Services;

public class JwtService : IJwtService
{
    private readonly byte[] _jwtEncodedSecret;
    private readonly UserManager<AppUser> _userManager;

    public JwtService(UserManager<AppUser> userManager)
    {
        _jwtEncodedSecret = EncryptHelpers.EncodeASCII(EnvVars.Jwt.Secret);
        _userManager = userManager;
    }

    public AccessToken GenerateAccessToken(IEnumerable<Claim> claims)
    {
        if (claims == null) throw new ArgumentNullException(null);

        var expireAt = DateTime.UtcNow.AddDays(EnvVars.Jwt.AccessExpiredDate);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = EnvVars.Jwt.Issuer,
            Audience = EnvVars.Jwt.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = expireAt,
            SigningCredentials = new SigningCredentials(
                key: new SymmetricSecurityKey(_jwtEncodedSecret),
                algorithm: SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AccessToken("Bearer", tokenHandler.WriteToken(token), expireAt.ToString("o"));
    }

    public async Task<AccessToken> GenerateAccessTokenAsync(AppUser user)
    {
        if (user == null)
            throw new ArgumentNullException(null);

        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
            {
                new Claim(UimClaimTypes.Id, user.Id),
                new Claim(UimClaimTypes.Name, user.UserName),
                new Claim(UimClaimTypes.Email, user.Email),
                new Claim(UimClaimTypes.Role, roles.First())
            };

        var expireAt = DateTime.UtcNow.AddDays(EnvVars.Jwt.AccessExpiredDate!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = EnvVars.Jwt.Issuer,
            Audience = EnvVars.Jwt.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = expireAt,
            SigningCredentials = new SigningCredentials(
                key: new SymmetricSecurityKey(_jwtEncodedSecret),
                algorithm: SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AccessToken("Bearer", tokenHandler.WriteToken(token), expireAt.ToString("o"));
    }

    public RefreshToken GenerateRefreshToken(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(null);

        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[64];
        rng.GetBytes(randomBytes);
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiredDate = DateTime.UtcNow.AddDays(EnvVars.Jwt.RefreshExpiredDate!),
            UserId = userId
        };
        return refreshToken;
    }

    public ClaimsPrincipal? GetClaimsPrincipal(string? token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token);
            if (jwtToken == null) return null;

            var validationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,

                ValidIssuers = EnvVars.ValidOrigins,
                ValidAudiences = EnvVars.ValidOrigins,
                IssuerSigningKey = new SymmetricSecurityKey(_jwtEncodedSecret)
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    public string? Validate(string? token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,

                ClockSkew = TimeSpan.Zero,
                ValidIssuers = EnvVars.ValidOrigins,
                ValidAudiences = EnvVars.ValidOrigins,
                IssuerSigningKey = new SymmetricSecurityKey(_jwtEncodedSecret),
            }, out SecurityToken validatedToken);

            var jwtToken = validatedToken as JwtSecurityToken;
            var userId = jwtToken?.Claims
                .FirstOrDefault(claim => claim.Type == UimClaimTypes.Id
                                        && claim.Value != null)?
                .Value;
            return userId;
        }
        // When fails validation do notthing
        catch
        {
            return null;
        }
    }

    public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken)
    {
        try
        {
            var googleId = EnvVars.SocialAuth.GoogleClientId;
            if (googleId == null)
                throw new HttpException(HttpStatusCode.BadRequest);

            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { googleId }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            return payload;
        }
        catch (InvalidJwtException)
        {
            throw new HttpException(HttpStatusCode.Forbidden);
        }
    }
}