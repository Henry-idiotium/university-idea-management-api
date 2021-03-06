namespace UIM.Core.Common.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly UimContext _context;

    public UnitOfWork(UimContext context)
    {
        _context = context;

        Comments = new CommentRepository(context);
        Tags = new TagRepository(context);
        Departments = new DepartmentRepository(context);
        Ideas = new IdeaRepository(context);
        Submissions = new SubmissionRepository(context);
        Users = new UserRepository(context);
    }

    public ICommentRepository Comments { get; private set; }
    public ITagRepository Tags { get; private set; }
    public IDepartmentRepository Departments { get; private set; }
    public IIdeaRepository Ideas { get; private set; }
    public ISubmissionRepository Submissions { get; private set; }
    public IUserRepository Users { get; private set; }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public TRepo Get<TRepo, TEntity>()
        where TRepo : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        var repo = (TRepo?)Activator.CreateInstance(typeof(TRepo), _context);
        if (repo == null)
            throw new ArgumentNullException("repo");

        return repo;
    }
}
