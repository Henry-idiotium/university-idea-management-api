namespace UIM.Core.Data.Repositories;

public class DepartmentRepository : Repository<Department>, IDepartmentRepository
{
    public DepartmentRepository(UimContext context) : base(context) { }

    public async Task<Department?> GetByNameAsync(string? name)
    {
        var depName = name?.ToLower();
        return await _context.Departments.FirstOrDefaultAsync(_ => _.Name.ToLower() == depName);
    }
}
