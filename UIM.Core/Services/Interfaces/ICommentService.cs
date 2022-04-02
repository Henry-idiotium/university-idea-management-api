namespace UIM.Core.Services.Interfaces;

public interface ICommentService
{
    Task CreateAsync(CreateCommentRequest request);
    Task EditAsync(UpdateCommentRequest request);
    Task<SieveResponse> FindAsync(string ideaId, SieveModel model);
    Task<CommentDetailsResponse> FindByIdAsync(string entityId);
    Task RemoveAsync(string entityId, string userId);
}
