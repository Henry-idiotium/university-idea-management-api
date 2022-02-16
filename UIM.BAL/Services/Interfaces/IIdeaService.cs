using System.Collections.Generic;
using System.Threading.Tasks;
using UIM.Model.Entities;

namespace UIM.BAL.Services.Interfaces
{
    public interface IIdeaService
    {
        Task CreateAsync(Idea idea);
        Task DeleteAsync(Idea idea);
        Task<IEnumerable<Idea>> ListAsync();
        Task<Idea> GetByIdAsync(int id);
    }
}