namespace UIM.Core.Services;

public class DepartmentService : Service, IDepartmentService
{
    public DepartmentService(IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork)
        : base(mapper, sieveProcessor, unitOfWork)
    {
    }

    public async Task CreateAsync(string name)
    {
        if (await _unitOfWork.Departments.GetByNameAsync(name) != null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var add = await _unitOfWork.Departments.AddAsync(new Department(name));
        if (!add.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task EditAsync(UpdateDepartmentRequest request)
    {
        var department = await _unitOfWork.Departments.GetByNameAsync(request.OldName);
        if (department == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        department.Name = request.NewName;

        var edit = await _unitOfWork.Departments.UpdateAsync(department);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }
    public IEnumerable<DepartmentDetailsResponse> FindAll()
        => _mapper.Map<IEnumerable<DepartmentDetailsResponse>>(_unitOfWork.Departments.Set);

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

    public async Task RemoveAsync(string departmentId)
    {
        var delete = await _unitOfWork.Departments.DeleteAsync(departmentId);
        if (!delete.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }
}