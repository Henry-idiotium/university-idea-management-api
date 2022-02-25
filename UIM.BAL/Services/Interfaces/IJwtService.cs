using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Google.Apis.Auth;
using UIM.Model.Dtos.Token;
using UIM.Model.Entities;

namespace UIM.BAL.Services.Interfaces
{
    public interface IJwtService
    {
        AccessToken GenerateAccessToken(IEnumerable<Claim> claims);
        RefreshToken GenerateRefreshToken(string userId);
        ClaimsPrincipal GetClaimsPrincipal(string token);
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken);
    }
}