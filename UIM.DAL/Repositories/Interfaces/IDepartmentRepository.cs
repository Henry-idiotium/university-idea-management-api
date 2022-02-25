using System.Collections.Generic;
using System.Threading.Tasks;
using UIM.Model.Entities;

namespace UIM.DAL.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<bool> AddAsync(string name);
        bool Edit(int id, string newName);
        Task<Department> GetByIdAsync(int? id);
        Task<Department> GetByNameAsync(string name);
        IEnumerable<Department> ListAll();
        Task<bool> RemoveAsync(Department department);
    }
}