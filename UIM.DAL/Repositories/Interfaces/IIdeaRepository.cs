using System.Collections.Generic;
using System.Threading.Tasks;
using UIM.Model.Entities;

namespace UIM.DAL.Repositories.Interfaces
{
    public interface IIdeaRepository
    {
        Task<bool> AddAsync(Idea idea);
        Task<Idea> GetByIdAsync(string ideaId);
        Task<IEnumerable<Idea>> ListAsync();
        Task<bool> RemoveAsync(Idea idea);
        Task<int> SaveAsync();
    }
}