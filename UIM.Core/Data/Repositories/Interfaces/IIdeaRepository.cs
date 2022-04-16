namespace UIM.Core.Data.Repositories.Interfaces;

public interface IIdeaRepository : IRepository<Idea>
{
    Task<bool> AddLikenessAsync(Like like);
    bool DeleteLikeness(Like like);
    Task<bool> AddToTagAsync(Idea idea, Tag tag);
    Task<IEnumerable<Idea>> GetBySubmissionAsync(string submissionId);
    IEnumerable<string> GetTags(string ideaId);
}
