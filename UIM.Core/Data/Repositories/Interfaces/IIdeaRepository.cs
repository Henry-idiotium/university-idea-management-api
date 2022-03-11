namespace UIM.Core.Data.Repositories.Interfaces;

public interface IIdeaRepository : IRepository<Idea, string>
{
    Task<IEnumerable<Idea>> GetBySubmissionAsync(string submissionId);
}