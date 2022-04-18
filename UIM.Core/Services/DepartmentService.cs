namespace UIM.Core.Services;

public class DepartmentService : Service, IDepartmentService
{
    public DepartmentService(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager
    ) : base(mapper, sieveProcessor, unitOfWork, userManager) { }

    public async Task CreateAsync(CreateDepartmentRequest request)
    {
        if (await _unitOfWork.Departments.GetByNameAsync(request.Name) != null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var dep = _mapper.Map<Department>(request);

        var add = await _unitOfWork.Departments.AddAsync(dep);
        if (!add.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task EditAsync(UpdateDepartmentRequest request)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(request.Id);
        if (department == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        _mapper.Map(request, department);

        var edit = await _unitOfWork.Departments.UpdateAsync(department);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public IEnumerable<SimpleDepartmentResponse> FindAll() =>
        _mapper.Map<IEnumerable<SimpleDepartmentResponse>>(_unitOfWork.Departments.Set);

    public async Task<SieveResponse> FindAsync(SieveModel model)
    {
        var sorted = _sieveProcessor.Apply(model, _unitOfWork.Departments.Set);
        if (sorted == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

        var mappedDep = _mapper.Map<IEnumerable<DepartmentDetailsResponse>>(sorted);
        return new(
            rows: mappedDep,
            index: model?.Page ?? 1,
            total: await _unitOfWork.Departments.CountAsync()
        );
    }

    public async Task<DepartmentDetailsResponse> FindByIdAsync(string id)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        var response = _mapper.Map<DepartmentDetailsResponse>(department);
        return response;
    }

    public async Task<DepartmentDetailsResponse> FindByNameAsync(string name)
    {
        var department = await _unitOfWork.Departments.GetByNameAsync(name);
        var response = _mapper.Map<DepartmentDetailsResponse>(department);
        return response;
    }

    public async Task RemoveAsync(string id)
    {
        var delete = await _unitOfWork.Departments.DeleteAsync(id);
        if (!delete.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }
}
