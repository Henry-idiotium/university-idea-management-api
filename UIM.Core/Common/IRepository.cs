namespace UIM.Core.Common;

public interface IRepository<TEntity, TIdentity>
    where TEntity : class, IEntity<TIdentity>
{
    Task<bool> AddAsync(TEntity entity);
    Task<bool> DeleteAsync(TIdentity entityId);
    Task<IEnumerable<TEntity>?> GetAllAsync();
    Task<TEntity?> GetByIdAsync(TIdentity entityId);
    Task<bool> UpdateAsync(TEntity entity);
}