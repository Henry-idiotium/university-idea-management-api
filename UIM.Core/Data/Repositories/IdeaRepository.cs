namespace UIM.Core.Data.Repositories;

public class IdeaRepository : Repository<Idea>, IIdeaRepository
{
    public IdeaRepository(UimContext context) : base(context) { }

    // DEPRECATED


    public async Task<IEnumerable<Idea>> GetBySubmissionAsync(string submissionId)
    {
        return await _context.Ideas
            .Where(_ => _.SubmissionId == submissionId)
            .ToListAsync();
    }
}