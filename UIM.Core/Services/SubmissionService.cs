namespace UIM.Core.Services;

public class SubmissionService
    : Service<
        CreateSubmissionRequest,
        UpdateSubmissionRequest,
        SubmissionDetailsResponse>,
    ISubmissionService
{
    public SubmissionService(IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork)
        : base(mapper, sieveProcessor, unitOfWork)
    {
    }

    public async Task AddIdeaToSubmissionAsync(AddIdeaRequest request)
    {
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

    public override async Task CreateAsync(CreateSubmissionRequest request)
    {
        var submission = _mapper.Map<Submission>(request);
        var isCreated = await _unitOfWork.Submissions.AddAsync(submission);
        if (!isCreated)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);
    }

    public override async Task EditAsync(string submissionId, UpdateSubmissionRequest request)
    {
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

    public override async Task<SieveResponse> FindAsync(SieveModel model)
    {
        if (model?.Page < 0 || model?.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var sortedSubs = _sieveProcessor.Apply(model, _unitOfWork.Submissions.Set);
        if (sortedSubs == null)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);

        var mappedSubs = _mapper.Map<List<SubmissionDetailsResponse>>(sortedSubs);

        return new(rows: mappedSubs,
            index: model?.Page,
            total: await _unitOfWork.Ideas.CountAsync());
    }

    public override async Task<SubmissionDetailsResponse> FindByIdAsync(string submissionId)
    {
        var submission = await _unitOfWork.Submissions.GetByIdAsync(submissionId);
        return _mapper.Map<SubmissionDetailsResponse>(submission);
    }

    public override async Task RemoveAsync(string submissionId)
    {
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