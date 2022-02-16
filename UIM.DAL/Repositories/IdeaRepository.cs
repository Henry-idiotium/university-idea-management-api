using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UIM.DAL.Data;
using UIM.DAL.Repositories.Interfaces;
using UIM.Model.Entities;

namespace UIM.DAL.Repositories
{
    public class IdeaRepository : IIdeaRepository
    {
        private readonly UimContext _context;

        public IdeaRepository(UimContext context) => _context = context;

        public async Task<bool> AddAsync(Idea idea)
        {
            await _context.Ideas.AddAsync(idea);
            var added = await SaveAsync();
            return added > 0;
        }

        public async Task<bool> RemoveAsync(Idea idea)
        {
            _context.Ideas.Remove(idea);
            var added = await SaveAsync();
            return added > 0;
        }

        public async Task<IEnumerable<Idea>> ListAsync() =>
            await _context.Ideas.ToListAsync();

        public async Task<Idea> GetByIdAsync(string ideaId) =>
            await _context.Ideas.FirstOrDefaultAsync(i => i.Id == ideaId);

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    }
}