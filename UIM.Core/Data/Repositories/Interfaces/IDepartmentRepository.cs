using System.Threading.Tasks;
using UIM.Core.Common;
using UIM.Core.Models.Entities;

namespace UIM.Core.Data.Repositories.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department, int>
    {
        Task<Department> GetByNameAsync(string name);
    }
}