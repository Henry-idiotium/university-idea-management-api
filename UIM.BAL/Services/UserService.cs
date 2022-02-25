using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using UIM.BAL.Services.Interfaces;
using UIM.Common;
using UIM.Common.Helpers;
using UIM.Common.ResponseMessages;
using UIM.DAL.Repositories.Interfaces;
using UIM.Model.Dtos.Auth;
using UIM.Model.Dtos.Common;
using UIM.Model.Dtos.Token;
using UIM.Model.Dtos.User;
using UIM.Model.Entities;

namespace UIM.BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly SieveOptions _sieveOptions;
        private readonly SieveProcessor _sieveProcessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(
            IJwtService jwtService,
            IMapper mapper,
            IUserRepository userRepository,
            UserManager<AppUser> userManager,
            IOptions<SieveOptions> sieveOptions,
            SieveProcessor sieveProcessor,
            IDepartmentRepository departmentRepository,
            RoleManager<IdentityRole> roleManager)
        {
            _jwtService = jwtService;
            _mapper = mapper;
            _userManager = userManager;
            _userRepository = userRepository;
            _sieveOptions = sieveOptions.Value;
            _sieveProcessor = sieveProcessor;
            _departmentRepository = departmentRepository;
            _roleManager = roleManager;
        }

        public async Task CreateAsync(CreateUserRequest request)
        {
            if (request.Password != request.ConfirmPassword)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            request.UserName ??= request.Email;
            var user = _mapper.Map<AppUser>(request);

            user.DepartmentId = (await _departmentRepository
                .GetByNameAsync(request.Department)).Id;

            if (user.DepartmentId == null)
                throw new ArgumentNullException(string.Empty);

            var userExist = _userRepository.ValidateExistence(user);
            if (userExist)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.UserAlreadyExists);

            await _userManager.CreateAsync(user, request.Password);
            await _userManager.AddToRoleAsync(user, request.Role);
        }

        public async Task DeleteAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(string.Empty);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            await _userManager.DeleteAsync(user);
        }

        public async Task<UserDetailsResponse> GetByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(string.Empty);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            return _mapper.Map<UserDetailsResponse>(user);
        }

        public async Task<TableResponse> GetUsersAsync(SieveModel model)
        {
            if (model?.Page < 0 || model?.PageSize < 1)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var users = new List<UserDetailsResponse>();
            foreach (var user in _userManager.Users)
            {
                var dep = await _departmentRepository.GetByIdAsync(user.DepartmentId);
                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                users.Add(new UserDetailsResponse
                {
                    Id = EncryptHelpers.EncodeBase64Url(user.Id),
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Department = dep.Name,
                    Role = role,
                    DateOfBirth = user.DateOfBirth,
                    CreatedAt = user.CreatedAt,
                });
            }

            var sortedUsers = _sieveProcessor.Apply(model, users.AsQueryable());

            var pageSize = model.PageSize ?? _sieveOptions.DefaultPageSize;

            return new TableResponse(sortedUsers, sortedUsers.Count(),
                currentPage: model.Page ?? 1,
                totalPages: (int)Math.Ceiling((float)users.Count / pageSize));
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
            if (request == null)
                throw new ArgumentNullException(string.Empty);

            var user = _userRepository.GetByRefreshToken(request.RefreshToken);
            var ownedRefreshToken = _userRepository.GetRefreshToken(request.RefreshToken);
            if (ownedRefreshToken == null)
                throw new ArgumentNullException(string.Empty);

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
                throw new ArgumentNullException(string.Empty);

            var userInfo = _mapper.Map<UserDetailsResponse>(user);
            var accessToken = _jwtService.GenerateAccessToken(principal.Claims);
            return new(userInfo, accessToken, newRefreshToken.Token);
        }

        public async Task UpdateInfoAsync(string userId, UpdateUserInfoRequest request)
        {
            if (request == null || userId == null)
                throw new ArgumentNullException(string.Empty);

            var user = await _userManager.FindByIdAsync(EncryptHelpers.DecodeBase64Url(userId));

            var newDepartment = await _departmentRepository.GetByIdAsync(request.NewDepartmentId);
            if (newDepartment == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var newRole = await _roleManager.Roles.FirstOrDefaultAsync(_ => _.Id == request.NewRoleId);
            if (newRole == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            await _userManager.RemoveFromRolesAsync(
                roles: await _userManager.GetRolesAsync(user),
                user: user);
            await _userManager.AddToRoleAsync(user, newRole.Id);

            user = _mapper.Map<AppUser>(request);
            user.DepartmentId = newDepartment.Id;

            await _userManager.UpdateAsync(user);
        }

        public async Task UpdatePasswordAsync(string userId, UpdateUserPasswordRequest request)
        {
            if (request == null || userId == null)
                throw new ArgumentNullException(string.Empty);

            var user = await _userManager.FindByIdAsync(userId);
            var pwdCorrect = await _userManager.CheckPasswordAsync(user, request.OldPassword);
            if (!pwdCorrect)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.IncorrectOldPassword);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (!resetResult.Succeeded)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.FailedResetPassword);
        }
    }
}