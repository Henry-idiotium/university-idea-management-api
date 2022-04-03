using System.Linq;

namespace UIM.Core.Services;

public class IdeaService : Service, IIdeaService
{
    public IdeaService(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager
    ) : base(mapper, sieveProcessor, unitOfWork, userManager) { }

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

    public async Task CreateAsync(CreateIdeaRequest request)
    {
        if (await _unitOfWork.Submissions.GetByIdAsync(request.SubmissionId) == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var idea = _mapper.Map<CreateIdeaRequest, Idea>(
            request,
            opt =>
                opt.AfterMap(
                    (src, dest) => dest.Attachments = _mapper.Map<List<Attachment>>(src.Attachments)
                )
        );

        var add = await _unitOfWork.Ideas.AddAsync(idea);
        if (!add.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);

        if (request.Tags != null)
            await AddTagsAsync(add.Entity!, request.Tags);
    }

    public async Task EditAsync(UpdateIdeaRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        var idea = await _unitOfWork.Ideas.GetByIdAsync(request.Id);

        if (
            !await _userManager.IsInRoleAsync(user, RoleNames.Admin)
            || idea?.UserId != user.Id
            || await _unitOfWork.Submissions.GetByIdAsync(request.SubmissionId) == null
        )
            throw new HttpException(HttpStatusCode.BadRequest);

        if (request.Tags != null)
            await AddTagsAsync(idea, request.Tags);

        _mapper.Map(
            request,
            idea,
            opt =>
                opt.AfterMap(
                    (src, dest) => dest.Attachments = _mapper.Map<List<Attachment>>(src.Attachments)
                )
        );

        var edit = await _unitOfWork.Ideas.UpdateAsync(idea);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task<SieveResponse> FindAsync(string submissionId, SieveModel model)
    {
        if (model.Page < 0 || model.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest);

        var sub = await _unitOfWork.Submissions.GetByIdAsync(submissionId);
        if (sub == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var ideas = _unitOfWork.Ideas.Set.Where(_ => _.SubmissionId == submissionId);
        var sortedIdeas = _sieveProcessor.Apply(model, ideas);
        if (sortedIdeas == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

        var mappedIdeas = new List<IdeaDetailsResponse>();
        foreach (var idea in sortedIdeas)
            mappedIdeas.Add(
                _mapper.Map<IdeaDetailsResponse>(
                    idea,
                    opt =>
                        opt.AfterMap(
                            (src, dest) =>
                            {
                                dest.User = _mapper.Map<UserDetailsResponse>(idea.User);
                                dest.Tags = _unitOfWork.Ideas.GetTags(idea.Id).ToArray();
                                dest.Submission = _mapper.Map<SubmissionDetailsResponse>(
                                    idea.Submission
                                );
                            }
                        )
                )
            );

        return new(
            rows: mappedIdeas,
            index: model?.Page,
            total: await _unitOfWork.Ideas.CountAsync()
        );
    }

    public async Task<IdeaDetailsResponse> FindByIdAsync(string ideaId)
    {
        var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);
        if (idea == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        // NOTE: {mappedIdea.User} may not work property
        var mappedIdea = _mapper.Map<Idea, IdeaDetailsResponse>(idea);
        if (idea.User != null)
            mappedIdea.User = _mapper.Map<UserDetailsResponse>(idea.User);

        return mappedIdea;
    }

    public async Task RemoveAsync(string userId, string ideaId)
    {
        var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);
        if (idea == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var user = await _userManager.FindByIdAsync(userId);
        if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin) || idea.UserId != user.Id)
            throw new HttpException(HttpStatusCode.BadRequest);

        var delete = await _unitOfWork.Ideas.DeleteAsync(ideaId);
        if (!delete.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }
}
