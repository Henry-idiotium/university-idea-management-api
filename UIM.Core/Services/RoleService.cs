namespace UIM.Core.Services;

public class RoleService : Service, IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager
    ) : base(mapper, sieveProcessor, unitOfWork, userManager)
    {
        _roleManager = roleManager;
    }

    public IEnumerable<RoleDetailsResponse> FindAll()
    {
        return _mapper.Map<List<RoleDetailsResponse>>(
            _roleManager.Roles.Where(_ => _.Name != EnvVars.Role.PwrUser)
        );
    }

    public async Task<RoleDetailsResponse> FindByIdAsync(string id)
    {
        var role = _mapper.Map<RoleDetailsResponse>(
            await _roleManager.Roles.FirstOrDefaultAsync(_ => _.Id == id)
        );

        return role;
    }

    public async Task<RoleDetailsResponse> FindByNameAsync(string name)
    {
        var role = _mapper.Map<RoleDetailsResponse>(await _roleManager.FindByNameAsync(name));
        return role;
    }
}
