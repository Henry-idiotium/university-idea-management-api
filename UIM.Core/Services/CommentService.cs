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

    public async Task<SieveResponse> FindAsync(string ideaId, SieveModel model)
    {
        if (model.Page < 0 || model.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest);

        var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);
        if (idea == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var comments = _unitOfWork.Comments.Set.Where(_ => _.UserId == ideaId);
        var sorted = _sieveProcessor.Apply(model, comments);
        if (sorted == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

        var mappedComments = _mapper.Map<IEnumerable<CommentDetailsResponse>>(sorted);
        return new(
            rows: mappedComments,
            index: model?.Page ?? 1,
            total: await _unitOfWork.Comments.CountAsync()
        );
    }

    public async Task<SieveResponse> FindManyByUserIdAsync(
        string ideaId,
        string userId,
        SieveModel model
    )
    {
        if (model.Page < 0 || model.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest);

        var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);
        var user = await _userManager.FindByIdAsync(userId);
        if (idea == null || user == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var comments = _unitOfWork.Comments.Set.Where(_ => _.UserId == ideaId);
        var sorted = _sieveProcessor.Apply(model, comments);
        if (sorted == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

        var mappedComments = _mapper.Map<IEnumerable<CommentDetailsResponse>>(sorted);
        return new(
            rows: mappedComments,
            index: model?.Page ?? 1,
            total: await _unitOfWork.Comments.CountAsync()
        );
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
