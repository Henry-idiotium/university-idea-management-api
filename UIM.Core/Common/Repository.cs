namespace UIM.Core.Common;

public abstract class Repository<TEntity, TIdentity> : IRepository<TEntity, TIdentity>
    where TEntity : class, IEntity<TIdentity>
{
    protected UimContext _context;
    protected DbSet<TEntity> _set;

    public Repository(UimContext context)
    {
        _context = context;
        _set = context.Set<TEntity>();
    }

    public async Task<bool> AddAsync(TEntity entity)
    {
        await _set.AddAsync(entity);
        var added = await _context.SaveChangesAsync();
        return added > 0;
    }

    public async Task<bool> DeleteAsync(TIdentity entityId)
    {
        var entity = await _set.FindAsync(entityId);
        if (entity == null) return false;

        _set.Remove(entity);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }

    public async Task<IEnumerable<TEntity>?> GetAllAsync() =>
        await _set.ToListAsync();

    public async Task<TEntity?> GetByIdAsync(TIdentity entityId) =>
        await _set.FindAsync(entityId);

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        var updated = await _context.SaveChangesAsync();
        return updated > 0;
    }
}