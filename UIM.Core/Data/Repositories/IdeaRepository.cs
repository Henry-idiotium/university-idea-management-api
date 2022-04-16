namespace UIM.Core.Data.Repositories;

public class IdeaRepository : Repository<Idea>, IIdeaRepository
{
    public IdeaRepository(UimContext context) : base(context) { }

    public IEnumerable<Like> GetLikes(string ideaId) =>
        _context.Likes.Where(_ => _.IdeaId == ideaId && _.IsLike);

    public IEnumerable<Like> GetDisikes(string ideaId) =>
        _context.Likes.Where(_ => _.IdeaId == ideaId && _.IsLike);

    public async Task<Like> AddLikenessAsync(Like like)
    {
        var entry = await _context.Likes.AddAsync(like);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public bool DeleteLikeness(Like like)
    {
        _context.Likes.Remove(like);
        var deleted = _context.SaveChanges();
        return deleted > 0;
    }

    public async Task<bool> AddToTagAsync(Idea idea, Tag tag)
    {
        await _context.IdeaTags.AddAsync(new IdeaTag { IdeaId = idea.Id, TagId = tag.Id });
        var added = await _context.SaveChangesAsync();
        return added > 0;
    }

    public void RemoveAllTags(Idea idea)
    {
        var ideaTags = _context.IdeaTags.Where(_ => _.IdeaId == idea.Id);
        if (ideaTags != null && ideaTags.Any())
            _context.IdeaTags.RemoveRange(ideaTags);
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
