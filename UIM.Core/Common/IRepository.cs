namespace UIM.Core.Common;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    DbSet<TEntity> Set { get; }
    Task<bool> AddAsync(TEntity entity);
    Task<int> CountAsync();
    Task<bool> DeleteAsync(string entityId);
    Task<TEntity?> GetByIdAsync(string entityId);
    Task<bool> UpdateAsync(TEntity entity);
}