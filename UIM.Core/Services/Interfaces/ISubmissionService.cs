namespace UIM.Core.Services.Interfaces;

public interface ISubmissionService
    : IService<
        CreateSubmissionRequest,
        UpdateSubmissionRequest,
        SubmissionDetailsResponse>
{
    Task AddIdeaAsync(AddIdeaRequest request);
}