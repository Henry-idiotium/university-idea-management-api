namespace UIM.Core.Services;

public class DepartmentService
    : Service<int,
        CreateDepartmentRequest,
        UpdateDepartmentRequest,
        DepartmentDetailsResponse>,
    IDepartmentService
{
    public DepartmentService(IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork)
        : base(mapper, sieveProcessor, unitOfWork)
    {
    }

    public override async Task CreateAsync(CreateDepartmentRequest request)
    {
        if (await _unitOfWork.Departments.GetByNameAsync(request.Name) != null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var added = await _unitOfWork.Departments.AddAsync(
            new Department { Name = request.Name });
        if (!added)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);
    }

    public override async Task EditAsync(int departmentId, UpdateDepartmentRequest request)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(departmentId);
        if (department == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        department.Name = request.Name;
        var edited = await _unitOfWork.Departments.UpdateAsync(department);
        if (!edited)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);
    }

    public override async Task<TableResponse> FindAsync(SieveModel model)
    {
        if (model?.Page < 0 || model?.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var sortedDeps = _sieveProcessor.Apply(model, _unitOfWork.Departments.Set);
        if (sortedDeps == null)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);

        var mappedDeps = _mapper.Map<List<DepartmentDetailsResponse>>(sortedDeps);

        return new(rows: mappedDeps,
            index: model?.Page,
            total: await _unitOfWork.Departments.CountAsync());
    }

    public override async Task<DepartmentDetailsResponse> FindByIdAsync(int departmentId)
    {
        var department = _mapper.Map<DepartmentDetailsResponse>(
            await _unitOfWork.Departments.GetByIdAsync(departmentId));
        return department;
    }

    public override async Task RemoveAsync(int departmentId)
    {
        var succeeded = await _unitOfWork.Departments.DeleteAsync(departmentId);
        if (!succeeded)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);
    }
}