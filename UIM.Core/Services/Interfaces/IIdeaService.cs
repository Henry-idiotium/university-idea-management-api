using UIM.Core.Models.Dtos.Like;

namespace UIM.Core.Services.Interfaces;

public interface IIdeaService
{
    Task<LikeDetailsResponse> AddLikenessAsync(CreateLikeRequest request);
    Task AddTagsAsync(Idea idea, string[] tags);
    Task<SimpleIdeaResponse> CreateAsync(CreateIdeaRequest request);
    Task DeleteLikenessAsync(string userId, string ideaId);
    Task EditAsync(UpdateIdeaRequest request);
    Task<SieveResponse> FindAsync(string submissionId, SieveModel model);
    Task<IdeaDetailsResponse> FindByIdAsync(string entityId);
    Task RemoveAsync(string userId, string ideaId);
}
