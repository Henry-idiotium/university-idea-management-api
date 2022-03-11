namespace UIM.Core.Data.Repositories;

public class CategoryRepository : Repository<Category, int>, ICategoryRepository
{
    public CategoryRepository(UimContext context) : base(context) { }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(_ => _.Name.ToLower() == name.ToLower());
    }
}