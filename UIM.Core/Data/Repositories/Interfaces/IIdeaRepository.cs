namespace UIM.Core.Data.Repositories.Interfaces;

public interface IIdeaRepository : IRepository<Idea>
{
    Task AddCommentAsync(Comment comment);
    Task AddLikenessAsync(Like like);
    Task<bool> AddToTagAsync(Idea idea, Tag tag);
    void DeleteComment(Comment comment);
    void DeleteLikeness(Like like);
    Task<IEnumerable<Idea>> GetBySubmissionAsync(string submissionId);
    IEnumerable<string> GetTags(string ideaId);
    void UpdateComment(Comment comment);
}
