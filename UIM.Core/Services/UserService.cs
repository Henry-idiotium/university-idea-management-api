namespace UIM.Core.Services;

public class UserService : Service, IUserService
{
    private readonly IEmailService _emailService;
    private readonly UserManager<AppUser> _userManager;

    public UserService(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        IEmailService emailService)
        : base(mapper, sieveProcessor, unitOfWork)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task AddToDepartmentAsync(AppUser user, string? department)
    {
        var depObj = await _unitOfWork.Departments.GetByNameAsync(department);
        if (depObj == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var userToBeAdded = await _userManager.FindByEmailAsync(user.Email)
            ?? await _userManager.FindByIdAsync(user.Id);

        userToBeAdded.DepartmentId = depObj.Id;
        var edit = await _userManager.UpdateAsync(userToBeAdded);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task CreateAsync(CreateUserRequest request)
    {
        var userExist = await _userManager.FindByEmailAsync(request.Email);
        if (userExist != null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var newUser = _mapper.Map<AppUser>(request);
        var password = AuthHelpers.GeneratePassword(8, true);

        var userCreated = await _userManager.CreateAsync(newUser, password);
        await AddToDepartmentAsync(newUser, request.Department);
        await _userManager.AddToRoleAsync(newUser, request.Role);

        if (!userCreated.Succeeded)
        {
            await _userManager.DeleteAsync(newUser);
            throw new HttpException(HttpStatusCode.InternalServerError);
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(newUser);
        var sendSucceeded = await _emailService.SendAuthInfoUpdatePasswordAsync(
            receiver: newUser,
            passwordResetToken: token,
            receiverPassword: password,
            senderFullName: "Cecilia McDermott",
            senderTitle: "Senior Integration Executive");

        if (!sendSucceeded)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.SentEmailFailed);
    }

    public async Task EditAsync(string id, UpdateUserRequest request)
    {
        var userToEdit = await _userManager.FindByIdAsync(id);
        if (userToEdit == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var oldRoles = await _userManager.GetRolesAsync(userToEdit);

        await _userManager.RemoveFromRolesAsync(userToEdit, oldRoles);
        await _userManager.AddToRoleAsync(userToEdit, request.Role);
        await AddToDepartmentAsync(userToEdit, request.Department);

        userToEdit = _mapper.Map<AppUser>(request);
        await _userManager.UpdateAsync(userToEdit);
    }

    public async Task<SieveResponse> FindAsync(SieveModel model)
    {
        if (model?.Page < 0 || model?.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest);

        var sortedUsers = _sieveProcessor.Apply(model, _userManager.Users);
        if (sortedUsers == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var mappedUsers = new List<UserDetailsResponse>();
        foreach (var user in sortedUsers)
        {
            var role = await _userManager.GetRolesAsync(user);
            var department = await _unitOfWork.Departments.GetByIdAsync(user.DepartmentId);

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

    public async Task<UserDetailsResponse> FindByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new HttpException(HttpStatusCode.BadRequest);

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

    public async Task RemoveAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var userIsAdmin =
            user.Email == EnvVars.System.PwrUserAuth.Email
            || user.UserName == EnvVars.System.PwrUserAuth.UserName;

        if (user == null || userIsAdmin)
            throw new HttpException(HttpStatusCode.BadRequest);

        await _userManager.DeleteAsync(user);
    }
}