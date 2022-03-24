namespace UIM.Core.Services.Interfaces;

public interface ISubmissionService
    : IReadService<SubmissionDetailsResponse>
    , IModifyService<CreateSubmissionRequest, UpdateSubmissionRequest>
{
    Task AddIdeaAsync(AddIdeaRequest request);
}