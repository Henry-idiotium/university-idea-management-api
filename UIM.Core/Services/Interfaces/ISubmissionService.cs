using System.Threading.Tasks;
using UIM.Core.Common;
using UIM.Core.Models.Dtos.Submission;

namespace UIM.Core.Services.Interfaces
{
    public interface ISubmissionService : IService<string, CreateSubmissionRequest, UpdateSubmissionRequest, SubmissionDetailsResponse>
    {
        Task AddIdeaToSubmissionAsync(AddIdeaRequest request);
    }
}