namespace UIM.Core.Services.Interfaces;

public interface ISubmissionService
    : IService<
        CreateSubmissionRequest,
        UpdateSubmissionRequest,
        SubmissionDetailsResponse>
{
    Task AddIdeaToSubmissionAsync(AddIdeaRequest request);
}