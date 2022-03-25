namespace UIM.Core.Services.Interfaces;

public interface IIdeaService : IReadService<IdeaDetailsResponse>
{
    Task AddTagsAsync(Idea idea, string[] tags);
    Task CreateAsync(string userId, CreateIdeaRequest request);
    Task EditAsync(string entityId, string userId, UpdateIdeaRequest request);
    Task RemoveAsync(string userId, string ideaId);
}