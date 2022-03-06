using System.Collections.Generic;
using System.Threading.Tasks;
using UIM.Core.Common;
using UIM.Core.Models.Entities;

namespace UIM.Core.Data.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, int>
    {
        Task<Category> GetByNameAsync(string name);
    }
}