using System.Net;
using System.Threading.Tasks;
using MapsterMapper;
using UIM.BAL.Services.Interfaces;
using UIM.Common;
using UIM.Common.ResponseMessages;
using UIM.DAL.Repositories.Interfaces;
using UIM.Model.Dtos;
using UIM.Model.Dtos.Auth;
using UIM.Model.Dtos.Token;
using UIM.Model.Entities;

namespace UIM.BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(
            IJwtService jwtService,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _jwtService = jwtService;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task AddRefreshTokenAsync(AppUser user, RefreshToken refreshToken)
        {
            if (refreshToken == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            if (!refreshToken.IsActive)
                throw new HttpException(HttpStatusCode.Forbidden,
                                        ErrorResponseMessages.Forbidden);

            await _userRepository.AddRefreshTokenAsync(user, refreshToken);
        }

        public async Task RevokeRefreshToken(string token)
        {
            var refreshToken = _userRepository.GetRefreshToken(token);
            if (!refreshToken.IsActive)
                throw new HttpException(HttpStatusCode.Forbidden,
                                        ErrorResponseMessages.Forbidden);

            await _userRepository.RevokeRefreshTokenAsync(refreshToken, "Revoked without replacement");
        }

        public async Task<AuthResponse> RotateTokensAsync(RotateTokenRequest request)
        {
            var user = _userRepository.GetByRefreshToken(request.RefreshToken);
            var ownedRefreshToken = _userRepository.GetRefreshToken(request.RefreshToken);
            if (ownedRefreshToken == null)
                throw new HttpException(HttpStatusCode.Unauthorized,
                                        ErrorResponseMessages.Unauthorized);

            if (ownedRefreshToken.IsRevoked)
                // revoke all descendant tokens in case this token has been compromised
                await _userRepository.RevokeRefreshTokenDescendantsAsync(ownedRefreshToken, user,
                    reason: $"Attempted reuse of revoked ancestor token: {request.RefreshToken}");

            if (!ownedRefreshToken.IsActive)
                throw new HttpException(HttpStatusCode.Forbidden,
                                        ErrorResponseMessages.Forbidden);

            // rotate token
            var newRefreshToken = _jwtService.GenerateRefreshToken(user.Id);
            await _userRepository.RevokeRefreshTokenAsync(
                token: ownedRefreshToken,
                reason: "Replaced by new token",
                replacedByToken: newRefreshToken.Token);
            await _userRepository.RemoveOutdatedRefreshTokensAsync(user);

            // Get principal from expired token
            var principal = _jwtService.GetClaimsPrincipal(request.AccessToken);
            if (principal == null)
                throw new HttpException(HttpStatusCode.Unauthorized,
                                        ErrorResponseMessages.Unauthorized);

            var userInfo = _mapper.Map<UserDetailsResponse>(user);
            var accessToken = _jwtService.GenerateAccessToken(principal.Claims);
            return new(userInfo, accessToken, newRefreshToken.Token);
        }
    }
}