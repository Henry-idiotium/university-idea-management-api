namespace UIM.Core.Services.Interfaces;

public interface ISubmissionService
{
    Task AddIdeaAsync(AddIdeaRequest request);
    Task CreateAsync(CreateSubmissionRequest request);
    Task EditAsync(UpdateSubmissionRequest request);
    Task<SieveResponse> FindAsync(SieveModel model);
    Task<SubmissionDetailsResponse> FindByIdAsync(string entityId);
    Task RemoveAsync(string entityId);
}
