namespace UIM.Core.Services.Interfaces;

public interface IIdeaService
{
    Task<MediumIdeaResponse> AddLikenessAsync(CreateLikeRequest request);
    Task AddTagsAsync(Idea idea, string[] tags);
    Task AddViewAsync(CreateViewRequest request);
    Task<SimpleIdeaResponse> CreateAsync(CreateIdeaRequest request);
    Task DeleteLikenessAsync(string userId, string ideaId);
    Task EditAsync(UpdateIdeaRequest request);
    Task<SieveResponse> FindAsync(string submissionId, string userId, SieveModel model);
    Task<IdeaDetailsResponse> FindByIdAsync(string entityId, string userId);
    Task RemoveAsync(string userId, string ideaId);
    Task<MediumIdeaResponse> UpdateLikenessAsync(CreateLikeRequest request);
}
