using System;
using UIM.Core.Data;
using UIM.Core.Data.Repositories;
using UIM.Core.Data.Repositories.Interfaces;

namespace UIM.Core.Common
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly UimContext _context;

        public ICategoryRepository Categories { get; private set; }
        public IDepartmentRepository Departments { get; private set; }
        public IIdeaRepository Ideas { get; private set; }
        public ISubmissionRepository Submissions { get; private set; }
        public IUserRepository Users { get; private set; }

        public UnitOfWork(UimContext context)
        {
            _context = context;

            Categories = new CategoryRepository(context);
            Departments = new DepartmentRepository(context);
            Ideas = new IdeaRepository(context);
            Submissions = new SubmissionRepository(context);
            Users = new UserRepository(context);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}