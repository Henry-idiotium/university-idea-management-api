namespace UIM.Core.Services.Interfaces;

public interface ICommentService
{
    Task CreateAsync(CreateCommentRequest request);
    Task EditAsync(UpdateCommentRequest request);
    Task<IEnumerable<CommentDetailsResponse>> FindAllAsync(string ideaId);
    Task<IEnumerable<CommentDetailsResponse>> FindAllByUserIdAsync(string ideaId, string userId);
    Task<CommentDetailsResponse> FindByIdAsync(string entityId);
    Task RemoveAsync(string entityId, string userId);
}
