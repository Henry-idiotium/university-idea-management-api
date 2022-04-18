namespace UIM.Core.Data.Repositories;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(UimContext context) : base(context) { }

    public override async Task<ContextModifyResult<Comment>> AddAsync(Comment comment)
    {
        var entry = await Set.AddAsync(comment);
        var added = await _context.SaveChangesAsync();
        if (added < 0)
            return new(false);

        return new(entry.Entity);
    }
}
