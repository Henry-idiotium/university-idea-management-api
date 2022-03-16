namespace UIM.Core.Common;

public abstract class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    protected UimContext _context;

    public Repository(UimContext context)
    {
        _context = context;
        Set = context.Set<TEntity>();
    }

    public DbSet<TEntity> Set { get; private set; }

    public async Task<bool> AddAsync(TEntity entity)
    {
        await Set.AddAsync(entity);
        var added = await _context.SaveChangesAsync();
        return added > 0;
    }

    public async Task<int> CountAsync() => await Set.CountAsync();

    public async Task<bool> DeleteAsync(string entityId)
    {
        var entity = await Set.FindAsync(entityId);
        if (entity == null) return false;

        Set.Remove(entity);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }

    public async Task<TEntity?> GetByIdAsync(string entityId) => await Set.FindAsync(entityId);

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        var updated = await _context.SaveChangesAsync();
        return updated > 0;
    }
}