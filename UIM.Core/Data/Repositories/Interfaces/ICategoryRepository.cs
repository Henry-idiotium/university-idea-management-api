namespace UIM.Core.Data.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category, int>
{
    Task<Category?> GetByNameAsync(string name);
}