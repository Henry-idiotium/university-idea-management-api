namespace UIM.Core.Data.Repositories.Interfaces;

public interface ITagRepository : IRepository<Tag>
{
    Task<Tag?> GetByNameAsync(string? name);
}