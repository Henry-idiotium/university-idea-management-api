namespace UIM.Core.Data.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);
}