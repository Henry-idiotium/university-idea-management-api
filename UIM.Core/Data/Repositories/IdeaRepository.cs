namespace UIM.Core.Data.Repositories;

public class IdeaRepository : Repository<Idea>, IIdeaRepository
{
    public IdeaRepository(UimContext context) : base(context) { }

    public async Task<bool> AddToTagAsync(Idea idea, Tag tag)
    {
        await _context.IdeaTags.AddAsync(new IdeaTag { IdeaId = idea.Id, TagId = tag.Id });
        var added = await _context.SaveChangesAsync();
        return added > 0;
    }

    public async Task<IEnumerable<Idea>> GetBySubmissionAsync(string submissionId)
    {
        return await Set.Where(_ => _.SubmissionId == submissionId).ToListAsync();
    }

    public IEnumerable<string> GetTags(string ideaId) =>
        from idea in _context.Ideas
        join ideaTag in _context.IdeaTags on idea.Id equals ideaTag.IdeaId
        join tag in _context.Tags on ideaTag.TagId equals tag.Id
        where ideaTag.IdeaId == ideaId
        select tag.Name;
}
