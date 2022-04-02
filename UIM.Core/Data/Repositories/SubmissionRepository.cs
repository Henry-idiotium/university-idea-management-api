namespace UIM.Core.Data.Repositories;

public class SubmissionRepository : Repository<Submission>, ISubmissionRepository
{
    public SubmissionRepository(UimContext context) : base(context) { }
}
