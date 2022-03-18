namespace UIM.Core.Data.Repositories;

public class TagRepository : Repository<Tag>, ITagRepository
{
    public TagRepository(UimContext context) : base(context) { }

    public async Task<Tag?> GetByNameAsync(string? name)
    {
        var tagName = name?.ToLower();
        return await _context.Tags.FirstOrDefaultAsync(_ => _.Name.ToLower() == tagName);
    }
}