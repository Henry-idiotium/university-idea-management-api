namespace UIM.Core.Services.Interfaces;

public interface ISubmissionService
{
    Task CreateAsync(CreateSubmissionRequest request);
    Task EditAsync(UpdateSubmissionRequest request);
    IEnumerable<SimpleSubmissionResponse> FindAll();
    Task<SieveResponse> FindAsync(SieveModel model);
    Task<SubmissionDetailsResponse> FindByIdAsync(string entityId);
    Task RemoveAsync(string entityId);
}
