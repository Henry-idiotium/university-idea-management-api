using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Google.Apis.Auth;
using UIM.Core.Models.Dtos.Token;
using UIM.Core.Models.Entities;

namespace UIM.Core.Services.Interfaces
{
    public interface IJwtService
    {
        AccessToken GenerateAccessToken(IEnumerable<Claim> claims);
        RefreshToken GenerateRefreshToken(string userId);
        ClaimsPrincipal GetClaimsPrincipal(string token);
        string Validate(string token);
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken);
    }
}