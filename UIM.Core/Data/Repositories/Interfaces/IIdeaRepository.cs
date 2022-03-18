namespace UIM.Core.Data.Repositories.Interfaces;

public interface IIdeaRepository : IRepository<Idea>
{
    Task<bool> AddToTagAsync(Idea idea, Tag tag);
    Task<IEnumerable<Idea>> GetBySubmissionAsync(string submissionId);
}