namespace UIM.Core.Data.Repositories.Interfaces;

public interface IIdeaRepository : IRepository<Idea>
{
    Task<Like> AddLikenessAsync(Like like);
    bool DeleteLikeness(Like like);
    Task<bool> AddToTagAsync(Idea idea, Tag tag);
    Task<IEnumerable<Idea>> GetBySubmissionAsync(string submissionId);
    IEnumerable<string> GetTags(string ideaId);
    void RemoveAllTags(Idea idea);
    IEnumerable<Like> GetLikes(string ideaId);
    IEnumerable<Like> GetDislikes(string ideaId);
    Like? GetLikenessByUser(string ideaId, string userId);
    Like UpdateLikeness(Like like);
    Task<bool> AddViewAsync(View view);
    IEnumerable<View> GetViews(string? ideaId = null);
    IEnumerable<Like> GetAllLikeness();
}
