using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using UIM.Core.Helpers;
using UIM.Core.ResponseMessages;
using UIM.Core.Services.Interfaces;
using UIM.Core.Models.Dtos.Token;
using UIM.Core.Models.Entities;

namespace UIM.Core.Services
{
    public class JwtService : IJwtService
    {
        private readonly byte[] _jwtEncodedSecret;

        public JwtService() => _jwtEncodedSecret = EncryptHelpers.EncodeASCII(EnvVars.Jwt.Secret);

        public AccessToken GenerateAccessToken(IEnumerable<Claim> claims)
        {
            if (claims == null)
                throw new ArgumentNullException(string.Empty);

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

        public RefreshToken GenerateRefreshToken(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(string.Empty);

            using var cryptoProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            cryptoProvider.GetBytes(randomBytes);
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                ExpiredDate = DateTime.UtcNow.AddDays(EnvVars.Jwt.RefreshExpiredDate),
                UserId = userId
            };
            return refreshToken;
        }

        public ClaimsPrincipal GetClaimsPrincipal(string token)
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

                    ValidIssuers = EnvVars.ValidLocations,
                    ValidAudiences = EnvVars.ValidLocations,
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

        public string Validate(string token)
        {
            if (token is null) return null;
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
                    ValidIssuers = EnvVars.ValidLocations,
                    ValidAudiences = EnvVars.ValidLocations,
                    IssuerSigningKey = new SymmetricSecurityKey(_jwtEncodedSecret),
                }, out SecurityToken validatedToken);

                var jwtToken = validatedToken as JwtSecurityToken;
                var userId = jwtToken.Claims
                    .FirstOrDefault(claim => claim.Type == "nameid"
                                            && claim.Value != null)
                    .Value;
                return userId;
            }
            // When fails validation do notthing
            catch { return null; }
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { EnvVars.SocialAuth.GoogleClientId }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                return payload;
            }
            catch (InvalidJwtException)
            {
                throw new HttpException(HttpStatusCode.Forbidden,
                    ErrorResponseMessages.Forbidden);
            }
        }
    }
}