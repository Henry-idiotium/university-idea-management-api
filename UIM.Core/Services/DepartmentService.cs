namespace UIM.Core.Services;

public class DepartmentService : Service, IDepartmentService
{
    public DepartmentService(IMapper mapper,
        IOptions<SieveOptions> sieveOptions,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork)
        : base(mapper, sieveOptions, sieveProcessor, unitOfWork)
    {
    }

    public async Task CreateAsync(CreateDepartmentRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(string.Empty);

        if (await _unitOfWork.Departments.GetByNameAsync(request.Name) != null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var added = await _unitOfWork.Departments.AddAsync(
            new Department { Name = request.Name });
        if (!added)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);
    }

    public async Task EditAsync(int departmentId, UpdateDepartmentRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(string.Empty);

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

    public async Task<TableResponse> FindAsync(SieveModel model)
    {
        if (model == null)
            throw new ArgumentNullException(string.Empty);

        if (model?.Page < 0 || model?.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var sortedDeps = _sieveProcessor.Apply(model, _unitOfWork.Departments.Set);
        if (sortedDeps == null)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);

        var mappedDeps = _mapper.Map<List<DepartmentDetailsResponse>>(sortedDeps);
        var pageSize = model?.PageSize ?? _sieveOptions.DefaultPageSize;
        var total = await _unitOfWork.Departments.CountAsync();

        return new(mappedDeps, total,
            currentPage: model?.Page ?? 1,
            totalPages: (int)Math.Ceiling((float)sortedDeps.Count() / pageSize));
    }

    public async Task<DepartmentDetailsResponse> FindByIdAsync(int departmentId)
    {
        var department = _mapper.Map<DepartmentDetailsResponse>(
            await _unitOfWork.Departments.GetByIdAsync(departmentId));
        return department;
    }

    public async Task RemoveAsync(int departmentId)
    {
        var succeeded = await _unitOfWork.Departments.DeleteAsync(departmentId);
        if (!succeeded)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);
    }
}