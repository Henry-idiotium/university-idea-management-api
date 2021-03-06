namespace UIM.Core.Services;

public class UserService : Service, IUserService
{
    private readonly IEmailService _emailService;

    public UserService(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        IEmailService emailService
    ) : base(mapper, sieveProcessor, unitOfWork, userManager)
    {
        _emailService = emailService;
    }

    public async Task AddToDepartmentAsync(AppUser user, string? department)
    {
        var depObj = await _unitOfWork.Departments.GetByNameAsync(department);
        if (depObj == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var userToBeAdded =
            await _userManager.FindByEmailAsync(user.Email)
            ?? await _userManager.FindByIdAsync(user.Id);

        userToBeAdded.DepartmentId = depObj.Id;
        var edit = await _userManager.UpdateAsync(userToBeAdded);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task CreateAsync(CreateUserRequest request)
    {
        var userExist = await _userManager.FindByEmailAsync(request.Email);
        if (userExist != null || request.Role == EnvVars.Role.PwrUser)
            throw new HttpException(HttpStatusCode.BadRequest);

        if (request.Avatar.IsNullOrEmpty())
            request.Avatar = await DiceBearHelpers.GetAvatarAsync();

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

        var sendSucceeded = await _emailService.SendWelcomeEmailAsync(
            receiver: newUser,
            receiverPassword: password
        );

        if (!sendSucceeded)
        {
            await _userManager.DeleteAsync(newUser);
            throw new HttpException(HttpStatusCode.InternalServerError);
        }
    }

    public async Task EditAsync(UpdateUserRequest request)
    {
        var userToEdit = await _userManager.FindByIdAsync(request.Id);
        var userIsAdmin =
            userToEdit.Email == EnvVars.PwrUserAuth.Email
            || userToEdit.UserName == EnvVars.PwrUserAuth.UserName;

        if (userIsAdmin || userToEdit == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var oldRoles = await _userManager.GetRolesAsync(userToEdit);

        await _userManager.RemoveFromRolesAsync(userToEdit, oldRoles);
        await _userManager.AddToRoleAsync(userToEdit, request.Role);
        await AddToDepartmentAsync(userToEdit, request.Department);

        _mapper.Map(request, userToEdit);
        var edited = await _userManager.UpdateAsync(userToEdit);
        if (!edited.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task<SieveResponse> FindAsync(SieveModel model)
    {
        var sortedUsers = _sieveProcessor.Apply(model, _userManager.Users);
        var mappedUsers = new List<UserDetailsResponse>();
        foreach (var user in sortedUsers)
        {
            var role = await _userManager.GetRolesAsync(user);
            var department = await _unitOfWork.Departments.GetByIdAsync(user.DepartmentId);
            var mappedUser = _mapper.Map<UserDetailsResponse>(user);
            mappedUser.Department = department?.Name;
            mappedUser.Role = role?.FirstOrDefault() ?? string.Empty;
            mappedUsers.Add(mappedUser);
        }
        return new(
            rows: mappedUsers,
            index: model?.Page ?? 1,
            total: await _userManager.Users.CountAsync()
        );
    }

    public async Task<UserDetailsResponse> FindByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var department = await _unitOfWork.Departments.GetByIdAsync(user.DepartmentId);
        var role = await _userManager.GetRolesAsync(user);
        var mappedUser = _mapper.Map<UserDetailsResponse>(user);
        mappedUser.Department = department?.Name;
        mappedUser.Role = role?.FirstOrDefault() ?? string.Empty;

        return mappedUser;
    }

    public async Task<UserDetailsResponse> FindByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var department = await _unitOfWork.Departments.GetByIdAsync(user.DepartmentId);
        var role = await _userManager.GetRolesAsync(user);
        var mappedUser = _mapper.Map<UserDetailsResponse>(user);
        mappedUser.Department = department?.Name;
        mappedUser.Role = role?.FirstOrDefault() ?? string.Empty;

        return mappedUser;
    }

    public async Task MockCreateAsync(CreateUserRequest request, string password)
    {
        var userExist = await _userManager.FindByEmailAsync(request.Email);
        if (userExist != null || request.Role == EnvVars.Role.PwrUser)
            throw new HttpException(HttpStatusCode.BadRequest);

        if (request.Avatar.IsNullOrEmpty())
            request.Avatar = await DiceBearHelpers.GetAvatarAsync();

        var newUser = _mapper.Map<AppUser>(request);
        var userCreated = await _userManager.CreateAsync(newUser, password);
        await AddToDepartmentAsync(newUser, request.Department);
        await _userManager.AddToRoleAsync(newUser, request.Role);

        if (!userCreated.Succeeded)
        {
            await _userManager.DeleteAsync(newUser);
            throw new HttpException(HttpStatusCode.InternalServerError);
        }
    }

    public async Task RemoveAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var userIsAdmin =
            user.Email == EnvVars.PwrUserAuth.Email
            || user.UserName == EnvVars.PwrUserAuth.UserName;

        if (userIsAdmin || user == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var deleted = await _userManager.DeleteAsync(user);
        if (!deleted.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }
}
