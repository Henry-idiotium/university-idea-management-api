namespace UIM.Core.Data.Repositories.Interfaces;

public interface ISubmissionRepository : IRepository<Submission>
{
    Task<bool> AddToTagAsync(Submission idea, Tag tag);
}