namespace UIM.Core.Services.Interfaces;

public interface IIdeaService
    : IService<
        CreateIdeaRequest,
        UpdateIdeaRequest,
        IdeaDetailsResponse>
{
    Task AddTagsAsync(Idea idea, string[] tags);
}