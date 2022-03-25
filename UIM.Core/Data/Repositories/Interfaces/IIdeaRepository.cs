namespace UIM.Core.Data.Repositories.Interfaces;

public interface IIdeaRepository : IRepository<Idea>
{
    Task<IEnumerable<Idea>> GetBySubmissionAsync(string submissionId);
}