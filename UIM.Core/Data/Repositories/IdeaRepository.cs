using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UIM.Core.Common;
using UIM.Core.Data.Repositories.Interfaces;
using UIM.Core.Models.Entities;

namespace UIM.Core.Data.Repositories
{
    public class IdeaRepository : Repository<Idea, string>, IIdeaRepository
    {
        public IdeaRepository(UimContext context) : base(context) { }

        public async Task<IEnumerable<Idea>> GetBySubmissionAsync(string submissionId)
        {
            return await _context.Ideas
                .Where(_ => _.SubmissionId == submissionId)
                .ToListAsync();
        }
    }
}