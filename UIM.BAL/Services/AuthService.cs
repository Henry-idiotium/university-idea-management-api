using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using UIM.BAL.Services.Interfaces;
using UIM.Common;
using UIM.Common.ResponseMessages;
using UIM.Model.Dtos;
using UIM.Model.Dtos.Auth;
using UIM.Model.Entities;

namespace UIM.BAL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;

        public AuthService(
            IJwtService jwtService,
            UserManager<AppUser> userManager,
            IMapper mapper,
            SignInManager<AppUser> signInManager,
            IUserService userService)
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _userService = userService;
        }

        public async Task<AuthResponse> ExternalLoginAsync(string provider, string idToken)
        {
            var payload = await _jwtService.VerifyGoogleToken(idToken);
            var info = new UserLoginInfo(provider, payload.Subject, provider);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user is null)
                throw new HttpException(HttpStatusCode.BadRequest,
                    ErrorResponseMessages.BadRequest);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };
            var accessToken = _jwtService.GenerateAccessToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken(user.Id).Token;
            var userInfo = _mapper.Map<UserDetailsResponse>(user);
            return new(userInfo, accessToken, refreshToken);
        }

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password,
                isPersistent: true,
                lockoutOnFailure: false);
            if (!result.Succeeded)
                throw new HttpException(HttpStatusCode.Unauthorized,
                    ErrorResponseMessages.FailedLogin);

            var user = await _userManager.FindByEmailAsync(email);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            var accessToken = _jwtService.GenerateAccessToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
            await _userService.AddRefreshTokenAsync(user, refreshToken);

            var userInfo = _mapper.Map<UserDetailsResponse>(user);
            return new(userInfo, accessToken, refreshToken.Token);
        }
    }
}