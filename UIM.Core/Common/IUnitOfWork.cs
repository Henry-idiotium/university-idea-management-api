
namespace UIM.Core.Common;

public interface IUnitOfWork
{
    IDepartmentRepository Departments { get; }
    IIdeaRepository Ideas { get; }
    ISubmissionRepository Submissions { get; }
    ITagRepository Tags { get; }
    IUserRepository Users { get; }

    TRepo Get<TRepo, TEntity>()
        where TEntity : class, IEntity
        where TRepo : IRepository<TEntity>;
}