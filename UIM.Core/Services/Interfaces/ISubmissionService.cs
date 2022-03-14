namespace UIM.Core.Services.Interfaces;

public interface ISubmissionService
    : IService<string,
        CreateSubmissionRequest,
        UpdateSubmissionRequest,
        SubmissionDetailsResponse>
{
    Task AddIdeaToSubmissionAsync(AddIdeaRequest request);
}