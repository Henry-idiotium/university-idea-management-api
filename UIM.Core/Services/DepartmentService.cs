namespace UIM.Core.Services;

public class DepartmentService : Service, IDepartmentService
{
    public DepartmentService(IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork)
        : base(mapper, sieveProcessor, unitOfWork)
    {
    }

    public async Task CreateAsync(CreateDepartmentRequest request)
    {
        if (await _unitOfWork.Departments.GetByNameAsync(request.Name) != null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var department = _mapper.Map<Department>(request);

        var add = await _unitOfWork.Departments.AddAsync(department);
        if (add.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task EditAsync(string departmentId, UpdateDepartmentRequest request)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(departmentId);
        if (department == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        _mapper.Map(request, department);

        var edit = await _unitOfWork.Departments.UpdateAsync(department);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task<SieveResponse> FindAsync(SieveModel model)
    {
        if (model?.Page < 0 || model?.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest);

        var sorted = _sieveProcessor.Apply(model, _unitOfWork.Departments.Set);
        if (sorted == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

        return new(
            index: model?.Page,
            rows: _mapper.Map<List<DepartmentDetailsResponse>>(sorted),
            total: await _unitOfWork.Departments.CountAsync());
    }

    public async Task<DepartmentDetailsResponse> FindByIdAsync(string departmentId)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(departmentId);
        var response = _mapper.Map<DepartmentDetailsResponse>(department);
        return response;
    }

    public async Task RemoveAsync(string departmentId)
    {
        var delete = await _unitOfWork.Departments.DeleteAsync(departmentId);
        if (!delete.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }
}