namespace UIM.Core.Data.Repositories.Interfaces;

public interface IDepartmentRepository : IRepository<Department, int>
{
    Task<Department?> GetByNameAsync(string name);
}