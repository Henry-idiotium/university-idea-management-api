using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UIM.Core.Common;
using UIM.Core.Data.Repositories.Interfaces;
using UIM.Core.Models.Entities;

namespace UIM.Core.Data.Repositories
{
    public class CategoryRepository : Repository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(UimContext context) : base(context) { }

        public async Task<Category> GetByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(_ => _.Name.ToLower() == name.ToLower());
        }
    }
}