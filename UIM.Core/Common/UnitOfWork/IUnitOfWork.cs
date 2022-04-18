namespace UIM.Core.Common.UnitOfWork;

public interface IUnitOfWork
{
    ICommentRepository Comments { get; }
    IDepartmentRepository Departments { get; }
    IIdeaRepository Ideas { get; }
    ISubmissionRepository Submissions { get; }
    ITagRepository Tags { get; }
    IUserRepository Users { get; }

    TRepo Get<TRepo, TEntity>()
        where TEntity : class, IEntity
        where TRepo : IRepository<TEntity>;
}
