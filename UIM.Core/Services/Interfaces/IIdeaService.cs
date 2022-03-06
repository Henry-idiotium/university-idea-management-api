using UIM.Core.Common;
using UIM.Core.Models.Dtos.Idea;

namespace UIM.Core.Services.Interfaces
{
    public interface IIdeaService
        : IService<string, CreateIdeaRequest, UpdateIdeaRequest, IdeaDetailsResponse>
    {

    }
}