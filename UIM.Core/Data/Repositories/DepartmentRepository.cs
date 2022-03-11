namespace UIM.Core.Data.Repositories;

public class DepartmentRepository : Repository<Department, int>, IDepartmentRepository
{
    public DepartmentRepository(UimContext context) : base(context) { }

    public async Task<Department?> GetByNameAsync(string name)
    {
        return await _context.Departments
            .FirstOrDefaultAsync(_ => _.Name.ToLower() == name.ToLower());
    }
}