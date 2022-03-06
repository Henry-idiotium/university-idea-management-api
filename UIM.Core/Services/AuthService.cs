using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UIM.Core.Common;
using UIM.Core.Helpers;
using UIM.Core.Models.Dtos.Auth;
using UIM.Core.Models.Dtos.Token;
using UIM.Core.Models.Entities;
using UIM.Core.ResponseMessages;
using UIM.Core.Services.Interfaces;

namespace UIM.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public AuthService(
            IJwtService jwtService,
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponse> ExternalLoginAsync(string provider, string idToken)
        {
            var payload = await _jwtService.VerifyGoogleToken(idToken);
            var info = new UserLoginInfo(provider, payload.Subject, provider);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
                throw new ArgumentNullException(string.Empty);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };
            var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
            await _unitOfWork.Users.AddRefreshTokenAsync(user, refreshToken);
            var accessToken = _jwtService.GenerateAccessToken(claims);

            return new(accessToken, refreshToken.Token);
        }

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var pwdCorrect = await _userManager.CheckPasswordAsync(user, password);
            if (!pwdCorrect)
                throw new HttpException(HttpStatusCode.Unauthorized,
                                        ErrorResponseMessages.Unauthorized);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            var accessToken = _jwtService.GenerateAccessToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
            await _unitOfWork.Users.AddRefreshTokenAsync(user, refreshToken);

            return new(accessToken, refreshToken.Token);
        }

        public async Task RevokeRefreshToken(string token)
        {
            var refreshToken = _unitOfWork.Users.GetRefreshToken(token);
            if (!refreshToken.IsActive)
                throw new HttpException(HttpStatusCode.Forbidden,
                                        ErrorResponseMessages.Forbidden);

            await _unitOfWork.Users.RevokeRefreshTokenAsync(refreshToken, "Revoked without replacement");
        }

        public async Task<AuthResponse> RotateTokensAsync(RotateTokenRequest request)
        {
            var user = _unitOfWork.Users.GetByRefreshToken(request.RefreshToken);
            var ownedRefreshToken = _unitOfWork.Users.GetRefreshToken(request.RefreshToken);
            if (ownedRefreshToken == null)
                throw new ArgumentNullException(string.Empty);

            if (ownedRefreshToken.IsRevoked)
                // revoke all descendant tokens in case this token has been compromised
                await _unitOfWork.Users.RevokeRefreshTokenDescendantsAsync(ownedRefreshToken, user,
                    reason: $"Attempted reuse of revoked ancestor token: {request.RefreshToken}");

            if (!ownedRefreshToken.IsActive)
                throw new HttpException(HttpStatusCode.Forbidden,
                                        ErrorResponseMessages.Forbidden);

            // rotate token
            var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
            await _unitOfWork.Users.RevokeRefreshTokenAsync(
                token: ownedRefreshToken,
                reason: "Replaced by new token",
                replacedByToken: refreshToken.Token);
            await _unitOfWork.Users.RemoveOutdatedRefreshTokensAsync(user);

            // Get principal from expired token
            var principal = _jwtService.GetClaimsPrincipal(request.AccessToken);
            if (principal == null)
                throw new ArgumentNullException(string.Empty);

            var accessToken = _jwtService.GenerateAccessToken(principal.Claims);
            return new(accessToken, refreshToken.Token);
        }
    }
}