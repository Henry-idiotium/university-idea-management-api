namespace UIM.Core.Services.Interfaces;

public interface ICommentService
{
    Task CreateAsync(CreateCommentRequest request);
    Task EditAsync(UpdateCommentRequest request);
    Task<SieveCommentResponse> FindAllAsync(string ideaId, bool isInitial);
    Task<CommentDetailsResponse> FindByIdAsync(string entityId);
    Task RemoveAsync(string entityId, string userId);
}
