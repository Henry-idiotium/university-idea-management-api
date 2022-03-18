namespace UIM.Core.Services;

public class IdeaService : Service, IIdeaService
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

    public async Task CreateAsync(CreateIdeaRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId) == null
            || await _unitOfWork.Submissions.GetByIdAsync(request.SubmissionId) == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var idea = _mapper.Map<Idea>(request);

        var add = await _unitOfWork.Ideas.AddAsync(idea);
        if (!add.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);

        if (request.Tags != null)
            await AddTagsAsync(add.Entity!, request.Tags);
    }

    public async Task AddTagsAsync(Idea idea, string[] tags)
    {
        foreach (var tagName in tags)
        {
            var tag = await _unitOfWork.Tags.GetByNameAsync(tagName);
            if (tag == null)
                throw new HttpException(HttpStatusCode.BadRequest);

            var added = await _unitOfWork.Ideas.AddToTagAsync(idea, tag);
            if (!added)
                throw new HttpException(HttpStatusCode.InternalServerError);
        }
    }

    public async Task EditAsync(string ideaId, UpdateIdeaRequest request)
    {
        var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);
        if (idea == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        if (await _unitOfWork.Submissions.GetByIdAsync(request.SubmissionId) == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        if (request.Tags != null)
            await AddTagsAsync(idea, request.Tags);

        _mapper.Map(request, idea);

        var edit = await _unitOfWork.Ideas.UpdateAsync(idea);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task<SieveResponse> FindAsync(SieveModel model)
    {
        if (model?.Page < 0 || model?.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest);

        var sortedIdeas = _sieveProcessor.Apply(model, _unitOfWork.Ideas.Set);
        if (sortedIdeas == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

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

    public async Task<IdeaDetailsResponse> FindByIdAsync(string ideaId)
    {
        var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);
        if (idea == null)
            throw new HttpException(HttpStatusCode.BadRequest);
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

    public async Task RemoveAsync(string ideaId)
    {
        var delete = await _unitOfWork.Ideas.DeleteAsync(ideaId);
        if (!delete.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }
}