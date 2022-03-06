using UIM.Core.Common;
using UIM.Core.Data.Repositories.Interfaces;
using UIM.Core.Models.Entities;

namespace UIM.Core.Data.Repositories
{
    public class SubmissionRepository : Repository<Submission, string>, ISubmissionRepository
    {
        public SubmissionRepository(UimContext context) : base(context) { }
    }
}