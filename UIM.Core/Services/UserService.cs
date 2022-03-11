namespace UIM.Core.Services;

public class UserService : Service, IUserService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public UserService(
        IMapper mapper,
        IOptions<SieveOptions> sieveOptions,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        RoleManager<IdentityRole> roleManager,
        UserManager<AppUser> userManager)
        : base(mapper,
            sieveOptions,
            sieveProcessor,
            unitOfWork)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task CreateAsync(CreateUserRequest request)
    {
        if (request.Password != request.ConfirmPassword)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        request.UserName ??= request.Email;
        var newUser = _mapper.Map<AppUser>(request);

        var userExist = _unitOfWork.Users.ValidateExistence(newUser);
        if (userExist)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var depExists = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId) != null;
        var roleExists = _roleManager.Roles.Where(_ => _.Id == request.RoleId).Any();

        if (!depExists || !roleExists)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        newUser.DepartmentId = request.DepartmentId;

        await _userManager.CreateAsync(newUser, request.Password);
        await _userManager.AddToRoleAsync(newUser, request.RoleId);
    }

    public async Task EditAsync(string userId, UpdateUserRequest request)
    {
        userId = EncryptHelpers.DecodeBase64Url(userId);
        var user = await _userManager.FindByIdAsync(EncryptHelpers.DecodeBase64Url(userId));

        var newDepartment = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
        var newRole = await _roleManager.Roles.FirstOrDefaultAsync(_ => _.Id == request.RoleId);

        if (newRole == null || newDepartment == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var oldRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, oldRoles);
        await _userManager.AddToRoleAsync(user, newRole.Id);

        user = _mapper.Map<AppUser>(request);
        user.DepartmentId = newDepartment.Id;

        await _userManager.UpdateAsync(user);

        if (request.Password == null) return;
        if (request.Password != request.ConfirmPassword)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, request.Password);

        if (!result.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);
    }

    public async Task<TableResponse> FindAsync(SieveModel model)
    {
        if (model?.Page < 0 || model?.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var users = _userManager.Users;
        var sortedUsers = _sieveProcessor.Apply(model, users.AsQueryable());

        var mappedUsers = new List<UserDetailsResponse>();
        foreach (var user in sortedUsers)
        {
            var department = user.DepartmentId == null ? null
                : await _unitOfWork.Departments.GetByIdAsync((int)user.DepartmentId);

            var role = await _userManager.GetRolesAsync(user);

            mappedUsers.Add(_mapper.Map<UserDetailsResponse>(user, opt =>
                opt.AfterMap((src, dest) =>
                {
                    dest.Department = department?.Name;
                    dest.Role = role.First();
                })));
        }

        var pageSize = model?.PageSize ?? _sieveOptions.DefaultPageSize;

        return new(mappedUsers, mappedUsers.Count,
            currentPage: model?.Page ?? 1,
            totalPages: (int)Math.Ceiling((float)users.Count() / pageSize));
    }

    public async Task<UserDetailsResponse> FindByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var department = user.DepartmentId == null ? null
            : await _unitOfWork.Departments.GetByIdAsync((int)user.DepartmentId);

        var role = await _userManager.GetRolesAsync(user);

        var mappedUser = _mapper.Map<UserDetailsResponse>(user, opt =>
            opt.AfterMap((src, dest) =>
            {
                dest.Department = department?.Name;
                dest.Role = role.First();
            }));

        return mappedUser;
    }

    public async Task RemoveAsync(string userId)
    {
        userId = EncryptHelpers.DecodeBase64Url(userId);
        var user = await _userManager.FindByIdAsync(userId);

        var userIsAdmin =
            user.Email == EnvVars.System.PwrUserAuth.Email
            || user.UserName == EnvVars.System.PwrUserAuth.UserName;

        if (user == null || userIsAdmin)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        await _userManager.DeleteAsync(user);
    }
}