namespace UIM.Core.Services;

public class SubmissionService : Service, ISubmissionService
{
    public SubmissionService(IMapper mapper,
        IOptions<SieveOptions> sieveOptions,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork)
        : base(mapper, sieveOptions, sieveProcessor, unitOfWork)
    {
    }

    public async Task AddIdeaToSubmissionAsync(AddIdeaRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(string.Empty);

        var submission = await _unitOfWork.Submissions.GetByIdAsync(request.SubmissionId);
        var idea = await _unitOfWork.Ideas.GetByIdAsync(request.SubmissionId);
        if (submission == null || idea == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        idea.SubmissionId = submission.Id;
        var edited = await _unitOfWork.Ideas.UpdateAsync(idea);
        if (!edited)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);
    }

    public async Task CreateAsync(CreateSubmissionRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(string.Empty);

        var submission = _mapper.Map<Submission>(request);
        var isCreated = await _unitOfWork.Submissions.AddAsync(submission);
        if (!isCreated)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);
    }

    public async Task EditAsync(string submissionId, UpdateSubmissionRequest request)
    {
        if (request == null
            || string.IsNullOrEmpty(submissionId))
            throw new ArgumentNullException(string.Empty);

        var oldSubmission = await _unitOfWork.Submissions.GetByIdAsync(submissionId);
        if (oldSubmission == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        _mapper.Map(request, oldSubmission);
        var isEdited = await _unitOfWork.Submissions.UpdateAsync(oldSubmission);
        if (!isEdited)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);
    }

    public async Task<TableResponse> FindAsync(SieveModel model)
    {
        if (model == null)
            throw new ArgumentNullException(string.Empty);

        if (model?.Page < 0 || model?.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var sortedSubs = _sieveProcessor.Apply(model, _unitOfWork.Submissions.Set);
        if (sortedSubs == null)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);

        var mappedSubs = _mapper.Map<List<SubmissionDetailsResponse>>(sortedSubs);

        var pageSize = model?.PageSize ?? _sieveOptions.DefaultPageSize;
        var total = await _unitOfWork.Submissions.CountAsync();

        return new(mappedSubs, mappedSubs.Count,
            currentPage: model?.Page ?? 1,
            totalPages: (int)Math.Ceiling((float)(total / pageSize)));
    }

    public async Task<SubmissionDetailsResponse> FindByIdAsync(string submissionId)
    {
        if (string.IsNullOrEmpty(submissionId))
            throw new ArgumentNullException(string.Empty);

        var submission = await _unitOfWork.Submissions.GetByIdAsync(submissionId);
        return _mapper.Map<SubmissionDetailsResponse>(submission);
    }

    public async Task RemoveAsync(string submissionId)
    {
        if (string.IsNullOrEmpty(submissionId))
            throw new ArgumentNullException(string.Empty);

        var subExists = await _unitOfWork.Submissions.GetByIdAsync(submissionId);
        if (subExists == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var isDeleted = await _unitOfWork.Submissions.DeleteAsync(submissionId);
        if (!isDeleted)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);
    }
}