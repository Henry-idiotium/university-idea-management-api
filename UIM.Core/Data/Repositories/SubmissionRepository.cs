namespace UIM.Core.Data.Repositories;

public class SubmissionRepository : Repository<Submission, string>, ISubmissionRepository
{
    public SubmissionRepository(UimContext context) : base(context) { }
}