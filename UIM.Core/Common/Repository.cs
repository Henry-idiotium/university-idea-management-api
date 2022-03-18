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

    public async Task<ContextModifyResult<TEntity>> AddAsync(TEntity entity)
    {
        var entry = await Set.AddAsync(entity);

        var added = await _context.SaveChangesAsync();
        if (added == 0)
            return new($"Unable to add {entity}");

        return new(entry.Entity);
    }

    public async Task<int> CountAsync() => await Set.CountAsync();

    public async Task<ContextModifyResult<TEntity>> DeleteAsync(string? entityId)
    {
        var entity = await Set.FindAsync(entityId);
        if (entity == null)
            return new($"{entityId} is already available");

        var entry = Set.Remove(entity);
        var delete = await _context.SaveChangesAsync();
        if (delete == 0)
            return new($"Unable to delete {entityId}");

        return new(entry.Entity);
    }

    public async Task<TEntity?> GetByIdAsync(string? entityId) => await Set.FindAsync(entityId);

    public async Task<ContextModifyResult<TEntity>> UpdateAsync(TEntity entity)
    {
        var entry = Set.Update(entity);

        var updated = await _context.SaveChangesAsync();
        if (updated == 0)
            return new($"Unable to delete {entity}");

        return new(entry.Entity);
    }
}