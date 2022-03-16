namespace UIM.Core.Services;

public class UserService
    : Service<
        CreateUserRequest,
        UpdateUserRequest,
        UserDetailsResponse>,
    IUserService
{
    private readonly IEmailService _emailService;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public UserService(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        RoleManager<IdentityRole> roleManager,
        UserManager<AppUser> userManager,
        IEmailService emailService)
        : base(mapper, sieveProcessor, unitOfWork)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _emailService = emailService;
    }

    public override async Task CreateAsync(CreateUserRequest request)
    {
        var dep = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
        var role = await _roleManager.FindByIdAsync(EncryptHelpers.DecodeBase64Url(request.RoleId));
        if (dep == null || role == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        request.UserName ??= request.Email;
        var newUser = _mapper.Map<AppUser>(request);

        var userExist = await _userManager.FindByEmailAsync(newUser.Email);
        if (userExist != null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        newUser.DepartmentId = request.DepartmentId;

        var password = AuthHelpers.GeneratePassword(8, true);

        var userCreated = await _userManager.CreateAsync(newUser, password);
        var roleAdded = await _userManager.AddToRoleAsync(newUser, role.Name);

        await ValidationDisposal(newUser, isValid: userCreated.Succeeded || roleAdded.Succeeded);

        var token = await _userManager.GeneratePasswordResetTokenAsync(newUser);
        var sendSucceeded = await _emailService.SendAuthInfoUpdatePasswordAsync(
            receiver: newUser,
            passwordResetToken: token,
            receiverPassword: password,
            senderFullName: "Cecilia McDermott",
            senderTitle: "Senior Integration Executive");

        await ValidationDisposal(newUser, isValid: sendSucceeded);
    }

    public override async Task EditAsync(string id, UpdateUserRequest request)
    {
        var userToEdit = await _userManager.FindByIdAsync(EncryptHelpers.DecodeBase64Url(id));

        var newDepartment = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
        var newRole = await _roleManager.FindByIdAsync(EncryptHelpers.DecodeBase64Url(request.RoleId));
        if (newRole == null || newDepartment == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var oldRoles = await _userManager.GetRolesAsync(userToEdit);
        await _userManager.RemoveFromRolesAsync(userToEdit, oldRoles);
        await _userManager.AddToRoleAsync(userToEdit, newRole.Id);

        userToEdit = _mapper.Map<AppUser>(request);
        userToEdit.DepartmentId = newDepartment.Id;

        await _userManager.UpdateAsync(userToEdit);

        if (request.Password == null) return;
        if (request.Password != request.ConfirmPassword)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var token = await _userManager.GeneratePasswordResetTokenAsync(userToEdit);

        var result = await _userManager.ResetPasswordAsync(userToEdit, token, request.Password);
        if (!result.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);
    }

    public override async Task<SieveResponse> FindAsync(SieveModel model)
    {
        if (model?.Page < 0 || model?.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var sortedUsers = _sieveProcessor.Apply(model, _userManager.Users);
        if (sortedUsers == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var mappedUsers = new List<UserDetailsResponse>();
        foreach (var user in sortedUsers)
        {
            var role = await _userManager.GetRolesAsync(user);
            var department = user.DepartmentId == null ? null
                : await _unitOfWork.Departments.GetByIdAsync(user.DepartmentId);

            mappedUsers.Add(_mapper.Map<UserDetailsResponse>(user, opt =>
                opt.AfterMap((src, dest) =>
                {
                    dest.Department = department?.Name;
                    dest.Role = role.First();
                })));
        }

        return new(rows: mappedUsers,
            index: model?.Page ?? 1,
            total: await _userManager.Users.CountAsync());
    }

    public override async Task<UserDetailsResponse> FindByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var department = user.DepartmentId == null ? null
            : await _unitOfWork.Departments.GetByIdAsync(user.DepartmentId);

        var role = await _userManager.GetRolesAsync(user);

        var mappedUser = _mapper.Map<UserDetailsResponse>(user, opt =>
            opt.AfterMap((src, dest) =>
            {
                dest.Department = department?.Name;
                dest.Role = role.First();
            }));

        return mappedUser;
    }

    public override async Task RemoveAsync(string userId)
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

    private async Task ValidationDisposal(AppUser user, bool isValid)
    {
        if (!isValid)
        {
            await _userManager.DeleteAsync(user);
            throw new HttpException(HttpStatusCode.InternalServerError);
        }
    }
}