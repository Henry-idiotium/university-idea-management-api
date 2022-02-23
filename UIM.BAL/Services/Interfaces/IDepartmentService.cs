using System.Threading.Tasks;
using Sieve.Models;
using UIM.Model.Dtos.Common;
using UIM.Model.Entities;

namespace UIM.BAL.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task AddAsync(string name);
        Task RemoveAsync(Department item);
        TableResponse GetDepartments(SieveModel model);
        Department Edit(int id, string newName);
    }
}