namespace UIM.Core.Common.Repository;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    DbSet<TEntity> Set { get; }
    Task<ContextModifyResult<TEntity>> AddAsync(TEntity entity);
    Task<int> CountAsync();
    Task<ContextModifyResult<TEntity>> DeleteAsync(string? entityId);
    Task<TEntity?> GetByIdAsync(string? entityId);
    Task<ContextModifyResult<TEntity>> UpdateAsync(TEntity entity);
}
