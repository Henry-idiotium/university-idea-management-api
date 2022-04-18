namespace UIM.Core.Data.Repositories;

public class IdeaRepository : Repository<Idea>, IIdeaRepository
{
    public IdeaRepository(UimContext context) : base(context) { }

    public IEnumerable<Like> GetAllLikeness() => _context.Likes;

    public IEnumerable<Like> GetLikes(string ideaId) =>
        _context.Likes.Where(_ => _.IdeaId == ideaId && _.IsLike);

    public IEnumerable<Like> GetDislikes(string ideaId) =>
        _context.Likes.Where(_ => _.IdeaId == ideaId && !_.IsLike);

    public Like? GetLikenessByUser(string ideaId, string userId) =>
        _context.Likes.Where(_ => _.IdeaId == ideaId).FirstOrDefault(_ => _.UserId == userId);

    public IEnumerable<View> GetViews(string ideaId) =>
        _context.Views.Where(_ => _.IdeaId == ideaId);

    public async Task<bool> AddViewAsync(View view)
    {
        try
        {
            await _context.Views.AddAsync(view);
            var added = await _context.SaveChangesAsync();
            return added > 0;
        }
        catch
        {
            return true;
        }
    }

    public async Task<Like> AddLikenessAsync(Like like)
    {
        var entry = await _context.Likes.AddAsync(like);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public Like UpdateLikeness(Like like)
    {
        var entry = _context.Likes.Update(like);
        _context.SaveChanges();
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
