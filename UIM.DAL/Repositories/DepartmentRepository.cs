using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UIM.DAL.Data;
using UIM.DAL.Repositories.Interfaces;
using UIM.Model.Entities;

namespace UIM.DAL.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly UimContext _context;

        public DepartmentRepository(UimContext context) => _context = context;

        public async Task<bool> AddAsync(string name)
        {
            await _context.Departments.AddAsync(new Department { Name = name });
            var added = await _context.SaveChangesAsync();
            return added > 0;
        }

        public bool Edit(int id, string newName)
        {
            _context.Departments.Update(new Department { Id = id, Name = newName });
            var edited = _context.SaveChanges();
            return edited > 0;
        }

        public async Task<Department> GetByIdAsync(int? id)
        {
            if (id == null) return new();
            return await _context.Departments.FindAsync(id);
        }

        public async Task<Department> GetByNameAsync(string name)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(_ => _.Name.ToLower() == name.ToLower());
        }

        public IEnumerable<Department> ListAll() => _context.Departments;

        public async Task<bool> RemoveAsync(Department department)
        {
            _context.Departments.Remove(department);
            var removed = await _context.SaveChangesAsync();
            return removed > 0;
        }
    }
}