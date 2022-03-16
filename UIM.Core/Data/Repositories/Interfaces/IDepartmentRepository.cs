namespace UIM.Core.Data.Repositories.Interfaces;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<Department?> GetByNameAsync(string name);
}