namespace UIM.Core.Common;

public interface IRepository<TEntity, TIdentity>
    where TEntity : class, IEntity<TIdentity>
{
    DbSet<TEntity> Set { get; }
    Task<bool> AddAsync(TEntity entity);
    Task<int> CountAsync();
    Task<bool> DeleteAsync(TIdentity entityId);
    Task<TEntity?> GetByIdAsync(TIdentity entityId);
    Task<bool> UpdateAsync(TEntity entity);
}