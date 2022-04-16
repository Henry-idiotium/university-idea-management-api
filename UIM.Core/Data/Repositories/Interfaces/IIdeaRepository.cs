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
    IEnumerable<Like> GetDisikes(string ideaId);
}
