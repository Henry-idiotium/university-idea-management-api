using UIM.Core.Data.Repositories.Interfaces;

namespace UIM.Core.Common
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IDepartmentRepository Departments { get; }
        IIdeaRepository Ideas { get; }
        ISubmissionRepository Submissions { get; }
        IUserRepository Users { get; }
    }
}