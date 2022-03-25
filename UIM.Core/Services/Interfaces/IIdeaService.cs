namespace UIM.Core.Services.Interfaces;

public interface IIdeaService : IReadService<IdeaDetailsResponse>
{
    Task CreateAsync(string userId, CreateIdeaRequest request);
    Task EditAsync(string entityId, string userId, UpdateIdeaRequest request);
    Task RemoveAsync(string userId, string ideaId);
}