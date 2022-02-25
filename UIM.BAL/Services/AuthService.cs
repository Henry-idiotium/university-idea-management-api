using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using UIM.BAL.Services.Interfaces;
using UIM.Common;
using UIM.Common.ResponseMessages;
using UIM.DAL.Repositories.Interfaces;
using UIM.Model.Dtos.Auth;
using UIM.Model.Dtos.User;
using UIM.Model.Entities;

namespace UIM.BAL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;

        public AuthService(
            IJwtService jwtService,
            UserManager<AppUser> userManager,
            IMapper mapper,
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository)
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _mapper = mapper;
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
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
            var accessToken = _jwtService.GenerateAccessToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
            await _userRepository.AddRefreshTokenAsync(user, refreshToken);

            var userInfo = _mapper.Map<UserDetailsResponse>(user);
            return new(userInfo, accessToken, refreshToken.Token);
        }

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var pwdCorrect = await _userManager.CheckPasswordAsync(user, password);
            if (!pwdCorrect)
                throw new HttpException(HttpStatusCode.Unauthorized,
                                        ErrorResponseMessages.FailedLogin);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            var accessToken = _jwtService.GenerateAccessToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
            await _userRepository.AddRefreshTokenAsync(user, refreshToken);

            var userInfo = _mapper.Map<UserDetailsResponse>(user);
            userInfo.Role = string.Join(",", await _userManager.GetRolesAsync(user));
            userInfo.Department = (await _departmentRepository.GetByIdAsync(user.DepartmentId)).Name;

            return new(userInfo, accessToken, refreshToken.Token);
        }
    }
}