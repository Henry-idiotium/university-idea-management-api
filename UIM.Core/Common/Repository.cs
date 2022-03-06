using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UIM.Core.Data;

namespace UIM.Core.Common
{
    public abstract class Repository<TEntity, TIdentity> : IRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TIdentity>
    {
        protected UimContext _context;

        public Repository(UimContext context) => _context = context;

        public async Task<bool> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            var added = await _context.SaveChangesAsync();
            return added > 0;
        }

        public async Task<bool> DeleteAsync(TIdentity entityId)
        {
            var entity = await _context.Set<TEntity>().FindAsync(entityId);
            if (entity == null) return false;

            _context.Set<TEntity>().Remove(entity);
            var deleted = await _context.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _context.Set<TEntity>().ToListAsync();

        public async Task<TEntity> GetByIdAsync(TIdentity entityId) =>
            await _context.Set<TEntity>().FindAsync(entityId);

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            var updated = await _context.SaveChangesAsync();
            return updated > 0;
        }
    }
}