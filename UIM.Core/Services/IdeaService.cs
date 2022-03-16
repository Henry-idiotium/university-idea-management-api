namespace UIM.Core.Services;

public class IdeaService
    : Service<
        CreateIdeaRequest,
        UpdateIdeaRequest,
        IdeaDetailsResponse>,
    IIdeaService
{
    private readonly UserManager<AppUser> _userManager;

    public IdeaService(IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager)
        : base(mapper, sieveProcessor, unitOfWork)
    {
        _userManager = userManager;
    }

    public override async Task CreateAsync(CreateIdeaRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId) == null
            || await _unitOfWork.Submissions.GetByIdAsync(request.SubmissionId) == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        if (request.CategoryId != null)
            if (await _unitOfWork.Categories.GetByIdAsync(request.CategoryId) == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

        var newIdea = _mapper.Map<Idea>(request);
        var succeeded = await _unitOfWork.Ideas.AddAsync(newIdea);
        if (!succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);
    }

    public override async Task EditAsync(string ideaId, UpdateIdeaRequest request)
    {
        var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);
        if (idea == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        if (await _unitOfWork.Submissions.GetByIdAsync(request.SubmissionId) == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        if (request.CategoryId != null)
            if (await _unitOfWork.Categories.GetByIdAsync(request.CategoryId) == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

        idea = _mapper.Map<Idea>(request);
        var edited = await _unitOfWork.Ideas.UpdateAsync(idea);
        if (!edited)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);
    }

    public override async Task<SieveResponse> FindAsync(SieveModel model)
    {
        if (model?.Page < 0 || model?.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var sortedIdeas = _sieveProcessor.Apply(model, _unitOfWork.Ideas.Set);
        if (sortedIdeas == null)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);

        var mappedIdeas = new List<IdeaDetailsResponse>();
        foreach (var idea in sortedIdeas)
        {
            mappedIdeas.Add(_mapper.Map<IdeaDetailsResponse>(idea, opt =>
                opt.AfterMap((src, dest) =>
                    dest.User = _mapper.Map<UserDetailsResponse>(idea.User))));
        }

        return new(rows: mappedIdeas,
            index: model?.Page,
            total: await _unitOfWork.Ideas.CountAsync());
    }

    public override async Task<IdeaDetailsResponse> FindByIdAsync(string ideaId)
    {
        var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);
        if (idea == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);
        IdeaDetailsResponse mappedIdea;
        if (idea.User == null)
            mappedIdea = _mapper.Map<Idea, IdeaDetailsResponse>(idea);
        else
        {
            mappedIdea = _mapper.Map<Idea, IdeaDetailsResponse>(idea, opt =>
                opt.AfterMap((src, dest) =>
                    dest.User = _mapper.Map<UserDetailsResponse>(idea.User)));
        }
        return mappedIdea;
    }

    public override async Task RemoveAsync(string ideaId)
    {
        var succeeded = await _unitOfWork.Ideas.DeleteAsync(ideaId);
        if (!succeeded)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);
    }
}