namespace UIM.Core.Services.Interfaces;

public interface IIdeaService : IReadService<IdeaDetailsResponse>
{
    Task AddTagsAsync(Idea idea, string[] tags);
    Task CreateAsync(CreateIdeaRequest request);
    Task EditAsync(UpdateIdeaRequest request);
    Task RemoveAsync(string userId, string ideaId);
}