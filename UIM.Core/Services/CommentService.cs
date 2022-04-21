namespace UIM.Core.Services;

public class CommentService : Service, ICommentService
{
    private readonly IEmailService _emailService;

    public CommentService(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        IEmailService emailService
    ) : base(mapper, sieveProcessor, unitOfWork, userManager)
    {
        _emailService = emailService;
    }

    public async Task CreateAsync(CreateCommentRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        var idea = await _unitOfWork.Ideas.GetByIdAsync(request.IdeaId);
        if (user == null || idea == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var comment = _mapper.Map<Comment>(request);

        var add = await _unitOfWork.Comments.AddAsync(comment);
        if (!add.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);

        var sendSucceeded = await _emailService.SendNotifySomeoneCommentedAsync(
            receiver: await _userManager.FindByIdAsync(idea.UserId),
            commentContent: comment.Content,
            commenter: user,
            receiverIdea: idea
        );
        if (!sendSucceeded)
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

    public async Task<SieveCommentResponse> FindAllAsync(string ideaId, bool isInitial)
    {
        var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);
        if (idea == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var comments = _unitOfWork.Comments.Set
            .Where(_ => _.IdeaId == ideaId)
            .OrderByDescending(_ => _.CreatedDate)
            .AsQueryable();

        var returnComments = isInitial ? comments.Take(3) : comments.Skip(3);

        var mappedComments = new List<CommentDetailsResponse>();
        foreach (var comment in returnComments ?? Enumerable.Empty<Comment>().AsQueryable())
        {
            var userComment = await _userManager.FindByIdAsync(comment.UserId);
            var mappedComment = _mapper.Map<CommentDetailsResponse>(comment);
            mappedComment.User = _mapper.Map<SimpleUserResponse>(userComment);
            mappedComment.Idea = _mapper.Map<SimpleIdeaResponse>(idea);
            mappedComments.Add(mappedComment);
        }
        return new(mappedComments, comments != null ? await comments.CountAsync() : 0);
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
