using UIM.Core.Models.Dtos.Like;

namespace UIM.Core.Services;

public class IdeaService : Service, IIdeaService
{
    private readonly IGoogleDriveService _driveService;

    public IdeaService(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        IGoogleDriveService driveService
    ) : base(mapper, sieveProcessor, unitOfWork, userManager)
    {
        _driveService = driveService;
    }

    public async Task AddLikenessAsync(CreateLikeRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        var idea = await _unitOfWork.Ideas.GetByIdAsync(request.IdeaId);
        if (idea == null || user == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var like = _mapper.Map<Like>(request);
        var add = await _unitOfWork.Ideas.AddLikenessAsync(like);
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
                        var uniqueFileName = $"{file.Name!}_{Guid.NewGuid()}";
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
        if (!add.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);

        if (request.Tags != null)
            await AddTagsAsync(add.Entity!, request.Tags);

        return _mapper.Map<SimpleIdeaResponse>(add.Entity);
    }

    public async Task EditAsync(UpdateIdeaRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        var idea = await _unitOfWork.Ideas.GetByIdAsync(request.Id);

        if (
            (!await _userManager.IsInRoleAsync(user, RoleNames.Admin) && idea?.UserId != user.Id)
            || await _unitOfWork.Submissions.GetByIdAsync(request.SubmissionId) == null
            || idea == null
        )
            throw new HttpException(HttpStatusCode.BadRequest);

        if (EnvVars.UseGoogleDrive)
        {
            // Update attachment
            if (request.Attachments != null && request.Attachments.Any())
            {
                foreach (var file in idea.Attachments)
                {
                    var fileExistsInRequest = request.Attachments.FirstOrDefault(
                        _ => _.FileId == file.FileId
                    );
                    if (fileExistsInRequest == null)
                    {
                        _driveService.DeleteFile(file.FileId);
                        idea.Attachments.Remove(file);
                    }
                }
                foreach (var file in request.Attachments)
                {
                    var fileIsExists = idea.Attachments.Where(_ => _.FileId == file.FileId).Any();
                    if (fileIsExists || !(file?.Data?.Length > 0))
                        continue;

                    using var stream = new MemoryStream();
                    file.Data.CopyTo(stream);

                    var uniqueFileName = $"{Guid.NewGuid()}_{file.Name!}";
                    var metadata = _driveService.UploadFile(
                        stream,
                        uniqueFileName,
                        file.Description!,
                        file.Mime!
                    );
                    metadata.IdeaId = idea.Id;
                    idea.Attachments.Add(metadata);
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

    public async Task<SieveResponse> FindAsync(string submissionId, SieveModel model)
    {
        var ideas = Enumerable.Empty<Idea>().AsQueryable();

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
            var user = await _userManager.FindByIdAsync(idea.UserId);
            var mappedIdea = _mapper.Map<IdeaDetailsResponse>(idea);
            mappedIdea.User = _mapper.Map<UserDetailsResponse>(user);
            mappedIdea.Tags = _unitOfWork.Ideas.GetTags(idea.Id).ToArray();
            mappedIdea.Attachments = _mapper.Map<AttachmentDetailsResponse[]>(idea.Attachments);
            mappedIdea.Submission = _mapper.Map<SubmissionDetailsResponse>(submission);
            mappedIdeas.Add(mappedIdea);
        }
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

        var mappedIdea = _mapper.Map<IdeaDetailsResponse>(idea);
        mappedIdea.User = _mapper.Map<UserDetailsResponse>(idea.User);
        mappedIdea.Tags = _unitOfWork.Ideas.GetTags(idea.Id).ToArray();
        mappedIdea.Attachments = _mapper.Map<AttachmentDetailsResponse[]>(idea.Attachments);
        mappedIdea.Submission = _mapper.Map<SubmissionDetailsResponse>(idea.Submission);

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
}
