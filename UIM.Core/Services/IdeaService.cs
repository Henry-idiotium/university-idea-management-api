namespace UIM.Core.Services;

public class IdeaService : Service, IIdeaService
{
    private readonly IGoogleDriveService _driveService;
    private readonly IEmailService _emailService;

    public IdeaService(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        IGoogleDriveService driveService,
        IEmailService emailService
    ) : base(mapper, sieveProcessor, unitOfWork, userManager)
    {
        _driveService = driveService;
        _emailService = emailService;
    }

    public async Task<MediumIdeaResponse> AddLikenessAsync(CreateLikeRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        var idea = await _unitOfWork.Ideas.GetByIdAsync(request.IdeaId);
        if (idea == null || user == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var like = _mapper.Map<Like>(request);
        like.ModifiedDate = DateTime.Now;
        like.CreatedDate = DateTime.Now;
        like.ModifiedBy = user.Email;
        like.CreatedBy = user.Email;

        var entity = await _unitOfWork.Ideas.AddLikenessAsync(like);
        if (entity == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

        var upIdea = _mapper.Map<MediumIdeaResponse>(await _unitOfWork.Ideas.GetByIdAsync(idea.Id));
        upIdea.Likes = _unitOfWork.Ideas.GetLikes(idea.Id).Count();
        upIdea.Dislikes = _unitOfWork.Ideas.GetDislikes(idea.Id).Count();
        upIdea.RequesterIsLike = _unitOfWork.Ideas.GetLikenessByUser(idea.Id, user.Id)?.IsLike;

        return upIdea;
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

    public async Task AddViewAsync(CreateViewRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        var idea = await _unitOfWork.Ideas.GetByIdAsync(request.IdeaId);
        if (idea == null || user == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var view = _mapper.Map<View>(request);
        view.CreatedDate = DateTime.Now;
        view.CreatedBy = user.Email;

        await _unitOfWork.Ideas.AddViewAsync(view);
    }

    public async Task<SimpleIdeaResponse> CreateAsync(CreateIdeaRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        var submission = await _unitOfWork.Submissions.GetByIdAsync(request.SubmissionId);
        if (user == null || submission == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var idea = _mapper.Map<Idea>(request);
        idea.CreatedBy = user.Email;
        idea.ModifiedBy = user.Email;

        if (EnvVars.UseGoogleDrive)
        {
            if (request.Attachments != null && request.Attachments.Any())
            {
                foreach (var file in request.Attachments)
                {
                    if (file?.Data?.Length > 0)
                    {
                        var uniqueFileName = $"{Guid.NewGuid().ToString()[..5]}_{file.Name!}";
                        var metadata = _driveService.UploadFile(
                            new MemoryStream(file.Data),
                            uniqueFileName,
                            file.Description!,
                            file.Mime!
                        );
                        idea.Attachments.Add(metadata);
                    }
                }
            }
        }

        var add = await _unitOfWork.Ideas.AddAsync(idea);
        if (!add.Succeeded || add.Entity == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

        if (request.Tags != null)
            await AddTagsAsync(add.Entity!, request.Tags);

        var receiver = (await _userManager.GetUsersInRoleAsync(EnvVars.Role.Supervisor))
            .Where(_ => _.DepartmentId == user.DepartmentId)
            .FirstOrDefault();

        var sendSucceeded = await _emailService.SendNotifyNewlyCreatedPostAsync(
            receiver: receiver ?? await _userManager.FindByEmailAsync(EnvVars.PwrUserAuth.Email),
            submission: submission,
            newIdea: add.Entity,
            author: user
        );
        if (!sendSucceeded)
            throw new HttpException(HttpStatusCode.InternalServerError);

        return _mapper.Map<SimpleIdeaResponse>(add.Entity);
    }

    public async Task DeleteLikenessAsync(string userId, string ideaId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);
        if (idea == null || user == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var deleted = _unitOfWork.Ideas.DeleteLikeness(
            new Like { IdeaId = ideaId, UserId = userId }
        );

        if (!deleted)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task EditAsync(UpdateIdeaRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        var idea = await _unitOfWork.Ideas.GetByIdAsync(request.Id);
        var submission = await _unitOfWork.Submissions.GetByIdAsync(request.SubmissionId);
        if (
            idea == null
            || submission == null
            || !await _userManager.IsInRoleAsync(user, RoleNames.Admin)
                && !await _userManager.IsInRoleAsync(user, RoleNames.Manager)
                && idea?.UserId != user.Id
        )
            throw new HttpException(HttpStatusCode.BadRequest);

        if (EnvVars.UseGoogleDrive)
        {
            // Update attachment
            if (request.Attachments != null && request.Attachments.Any())
            {
                idea.Attachments = new();
                foreach (var file in idea.Attachments)
                    _driveService.DeleteFile(file.FileId);

                foreach (var file in request.Attachments)
                {
                    if (file?.Data?.Length > 0)
                    {
                        var uniqueFileName = $"{Guid.NewGuid().ToString()[..5]}_{file.Name!}";
                        var metadata = _driveService.UploadFile(
                            new MemoryStream(file.Data),
                            uniqueFileName,
                            file.Description!,
                            file.Mime!
                        );
                        metadata.IdeaId = idea.Id;
                        idea.Attachments.Add(metadata);
                    }
                }
            }
        }

        if (request.Tags != null)
        {
            _unitOfWork.Ideas.RemoveAllTags(idea);
            await AddTagsAsync(idea, request.Tags);
        }

        _mapper.Map(request, idea);

        var edit = await _unitOfWork.Ideas.UpdateAsync(idea);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task<SieveResponse> FindAsync(string submissionId, string userId, SieveModel model)
    {
        var ideas = Enumerable.Empty<Idea>().AsQueryable();
        var requestUser = await _userManager.FindByIdAsync(userId);

        if (
            submissionId != string.Empty
            && await _unitOfWork.Submissions.GetByIdAsync(submissionId) == null
        )
            throw new HttpException(HttpStatusCode.BadRequest);

        if (submissionId != string.Empty)
            ideas = _unitOfWork.Ideas.Set.Where(_ => _.SubmissionId == submissionId);
        else
            ideas = _unitOfWork.Ideas.Set;

        var sortedIdeas = _sieveProcessor.Apply(model, ideas);
        if (sortedIdeas == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

        var mappedIdeas = new List<IdeaDetailsResponse>();
        foreach (var idea in sortedIdeas)
        {
            var submission = await _unitOfWork.Submissions.GetByIdAsync(idea.SubmissionId);
            var ideaUser = await _userManager.FindByIdAsync(idea.UserId);
            var mappedIdea = _mapper.Map<IdeaDetailsResponse>(idea);

            mappedIdea.User = _mapper.Map<UserDetailsResponse>(ideaUser);
            mappedIdea.Tags = _unitOfWork.Ideas.GetTags(idea.Id).ToArray();
            mappedIdea.Likes = _unitOfWork.Ideas.GetLikes(idea.Id).Count();
            mappedIdea.Dislikes = _unitOfWork.Ideas.GetDislikes(idea.Id).Count();
            mappedIdea.Attachments = _mapper.Map<AttachmentDetailsResponse[]>(idea.Attachments);
            mappedIdea.Submission = _mapper.Map<SubmissionDetailsResponse>(submission);
            mappedIdea.Views = _unitOfWork.Ideas.GetViews(idea.Id).Count();

            mappedIdea.RequesterIsLike = _unitOfWork.Ideas.GetLikenessByUser(
                idea.Id,
                requestUser.Id
            )?.IsLike;

            mappedIdea.CommentsCount = _unitOfWork.Comments.Set
                .Where(_ => _.IdeaId == idea.Id)
                .Count();

            mappedIdeas.Add(mappedIdea);
        }

        return new(
            rows: mappedIdeas,
            index: model?.Page,
            total: submissionId.IsNullOrEmpty()
              ? await _unitOfWork.Ideas.CountAsync()
              : await ideas.Where(_ => _.SubmissionId == submissionId).CountAsync()
        );
    }

    public async Task<IdeaDetailsResponse> FindByIdAsync(string ideaId, string userId)
    {
        var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);
        var requestUser = await _userManager.FindByIdAsync(userId);

        if (idea == null || requestUser == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var submission = await _unitOfWork.Submissions.GetByIdAsync(idea.SubmissionId);
        var ideaUser = await _userManager.FindByIdAsync(idea.UserId);
        var mappedIdea = _mapper.Map<IdeaDetailsResponse>(idea);

        mappedIdea.User = _mapper.Map<UserDetailsResponse>(ideaUser);
        mappedIdea.Tags = _unitOfWork.Ideas.GetTags(idea.Id).ToArray();
        mappedIdea.Likes = _unitOfWork.Ideas.GetLikes(idea.Id).Count();
        mappedIdea.Dislikes = _unitOfWork.Ideas.GetDislikes(idea.Id).Count();
        mappedIdea.Attachments = _mapper.Map<AttachmentDetailsResponse[]>(idea.Attachments);
        mappedIdea.Submission = _mapper.Map<SubmissionDetailsResponse>(submission);
        mappedIdea.Views = _unitOfWork.Ideas.GetViews(idea.Id).Count();

        mappedIdea.RequesterIsLike = _unitOfWork.Ideas.GetLikenessByUser(
            idea.Id,
            requestUser.Id
        )?.IsLike;

        mappedIdea.CommentsCount = _unitOfWork.Comments.Set.Where(_ => _.IdeaId == idea.Id).Count();

        return mappedIdea;
    }

    public async Task RemoveAsync(string userId, string ideaId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var idea = await _unitOfWork.Ideas.GetByIdAsync(ideaId);

        if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin) && idea?.UserId != user.Id)
            throw new HttpException(HttpStatusCode.BadRequest);

        if (idea?.Attachments.Any() == true)
        {
            foreach (var file in idea.Attachments)
            {
                _driveService.DeleteFile(file.FileId);
            }
        }

        var delete = await _unitOfWork.Ideas.DeleteAsync(ideaId);
        if (!delete.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }

    public async Task<MediumIdeaResponse> UpdateLikenessAsync(CreateLikeRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        var idea = await _unitOfWork.Ideas.GetByIdAsync(request.IdeaId);
        if (idea == null || user == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var like = _unitOfWork.Ideas.GetLikenessByUser(idea.Id, user.Id);
        Like entity;

        if (like == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

        if (request.IsLike == null)
            _unitOfWork.Ideas.DeleteLikeness(like);
        else
        {
            entity = _unitOfWork.Ideas.UpdateLikeness(_mapper.Map(request, like));
            if (entity == null)
                throw new HttpException(HttpStatusCode.InternalServerError);
        }

        var upIdea = _mapper.Map<MediumIdeaResponse>(await _unitOfWork.Ideas.GetByIdAsync(idea.Id));
        upIdea.Likes = _unitOfWork.Ideas.GetLikes(idea.Id).Count();
        upIdea.Dislikes = _unitOfWork.Ideas.GetDislikes(idea.Id).Count();
        upIdea.RequesterIsLike = _unitOfWork.Ideas.GetLikenessByUser(idea.Id, user.Id)?.IsLike;

        return upIdea;
    }
}
