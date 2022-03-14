namespace UIM.Core.Services;

public class RoleService
    : Service<string,
        ICreateRequest,
        IUpdateRequest,
        RoleDetailsResponse>,
    IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        RoleManager<IdentityRole> roleManager)
        : base(mapper, sieveProcessor, unitOfWork)
    {
        _roleManager = roleManager;
    }

    public override Task CreateAsync(ICreateRequest request) => throw new NotImplementedException();
    public override Task EditAsync(string entityId, IUpdateRequest request) => throw new NotImplementedException();

    public override async Task<TableResponse> FindAsync(SieveModel model)
    {
        if (model?.Page < 0 || model?.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var sortedRoles = _sieveProcessor.Apply(model, _roleManager.Roles);
        if (sortedRoles == null)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);

        var mappedRoles = _mapper.Map<List<RoleDetailsResponse>>(sortedRoles);
        var count = (await _roleManager.Roles.ToListAsync()).Count;

        return new(rows: mappedRoles,
            index: model?.Page,
            total: count);
    }

    public override async Task<RoleDetailsResponse> FindByIdAsync(string id)
    {
        var roleId = EncryptHelpers.DecodeBase64Url(id);
        var role = _mapper.Map<RoleDetailsResponse>(
            await _roleManager.Roles.FirstOrDefaultAsync(_ => _.Id == roleId));

        return role;
    }

    public async Task<RoleDetailsResponse> FindByNameAsync(string name)
    {
        var role = _mapper.Map<RoleDetailsResponse>(await _roleManager.FindByNameAsync(name));
        return role;
    }

    public override Task RemoveAsync(string entityId) => throw new NotImplementedException();
}