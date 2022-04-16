namespace UIM.Core.Services;

public class CommentService : Service, ICommentService
{
    public CommentService(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager
    ) : base(mapper, sieveProcessor, unitOfWork, userManager) { }

    public async Task CreateAsync(CreateCommentRequest request)
    {
        if (await _unitOfWork.Ideas.GetByIdAsync(request.IdeaId) == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var comment = _mapper.Map<Comment>(request);

        var add = await _unitOfWork.Comments.AddAsync(comment);
        if (!add.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task EditAsync(UpdateCommentRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        var comment = await _unitOfWork.Comments.GetByIdAsync(request.Id);

        if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin) || comment?.UserId != user.Id)
            throw new HttpException(HttpStatusCode.BadRequest);

        _mapper.Map(request, comment);

        var edit = await _unitOfWork.Comments.UpdateAsync(comment);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task<IEnumerable<CommentDetailsResponse>> FindAllAsync(string ideaId)
    {
        var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);
        if (idea == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var comments = _unitOfWork.Comments.Set.Where(_ => _.IdeaId == ideaId);
        var mappedComments = new List<CommentDetailsResponse>();
        foreach (var comment in comments ?? Enumerable.Empty<Comment>().AsQueryable())
        {
            var mappedComment = _mapper.Map<CommentDetailsResponse>(comment);
            mappedComment.User = _mapper.Map<SimpleUserResponse>(comment.User);
            mappedComment.Idea = _mapper.Map<SimpleIdeaResponse>(comment.Idea);

            mappedComments.Add(mappedComment);
        }
        return mappedComments;
    }

    // DEPRECATED: STUPID LOGIC
    public async Task<IEnumerable<CommentDetailsResponse>> FindAllByUserIdAsync(
        string ideaId,
        string userId
    )
    {
        var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);
        var user = await _userManager.FindByIdAsync(userId);
        if (idea == null || user == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var comments = _unitOfWork.Comments.Set
            .Where(_ => _.IdeaId == ideaId)
            .Where(_ => _.UserId == userId);
        var mappedComments = new List<CommentDetailsResponse>();
        foreach (var comment in comments ?? Enumerable.Empty<Comment>().AsQueryable())
        {
            var mappedComment = _mapper.Map<CommentDetailsResponse>(comment);
            mappedComment.User = _mapper.Map<SimpleUserResponse>(user);
            mappedComment.Idea = _mapper.Map<SimpleIdeaResponse>(comment.Idea);

            mappedComments.Add(mappedComment);
        }
        return mappedComments;
    }

    public async Task<CommentDetailsResponse> FindByIdAsync(string id)
    {
        var comment = await _unitOfWork.Comments.GetByIdAsync(id);
        var response = _mapper.Map<CommentDetailsResponse>(comment);
        return response;
    }

    public async Task RemoveAsync(string id, string userId)
    {
        var comment = await _unitOfWork.Comments.GetByIdAsync(id);
        if (comment == null || await _userManager.FindByIdAsync(comment?.UserId) == null)
        {
            throw new HttpException(HttpStatusCode.BadRequest);
        }

        var delete = await _unitOfWork.Comments.DeleteAsync(id);
        if (!delete.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }
}
