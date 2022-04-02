namespace UIM.Core.Services.Interfaces;

public interface IIdeaService
{
    Task AddTagsAsync(Idea idea, string[] tags);
    Task CreateAsync(CreateIdeaRequest request);
    Task EditAsync(UpdateIdeaRequest request);
    Task<SieveResponse> FindAsync(SieveModel model);
    Task<IdeaDetailsResponse> FindByIdAsync(string entityId);
    Task RemoveAsync(string userId, string ideaId);
}
