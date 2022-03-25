namespace UIM.Core.Data.Repositories;

public class SubmissionRepository : Repository<Submission>, ISubmissionRepository
{
    public SubmissionRepository(UimContext context) : base(context) { }

    public async Task<bool> AddToTagAsync(Submission submission, Tag tag)
    {
        await _context.SubmissionTags.AddAsync(new SubmissionTag { SubmissionId = submission.Id, TagId = tag.Id });
        var added = await _context.SaveChangesAsync();
        return added > 0;
    }
}