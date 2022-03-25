namespace UIM.Core.Services.Interfaces;

public interface ISubmissionService
    : IReadService<SubmissionDetailsResponse>
    , IModifyService<CreateSubmissionRequest, UpdateSubmissionRequest>
{
    Task AddTagsAsync(Submission submission, string[] tags);
    Task AddIdeaAsync(AddIdeaRequest request);
}