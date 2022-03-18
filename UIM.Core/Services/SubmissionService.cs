namespace UIM.Core.Services;

public class SubmissionService : Service, ISubmissionService
{
    public SubmissionService(IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork)
        : base(mapper, sieveProcessor, unitOfWork)
    {
    }

    public async Task AddIdeaAsync(AddIdeaRequest request)
    {
        var submission = await _unitOfWork.Submissions.GetByIdAsync(request.SubmissionId);
        var idea = await _unitOfWork.Ideas.GetByIdAsync(request.SubmissionId);
        if (submission == null || idea == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        idea.SubmissionId = submission.Id;
        var edit = await _unitOfWork.Ideas.UpdateAsync(idea);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task CreateAsync(CreateSubmissionRequest request)
    {
        var submission = _mapper.Map<Submission>(request);
        var add = await _unitOfWork.Submissions.AddAsync(submission);
        if (add.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }

    public async Task EditAsync(string submissionId, UpdateSubmissionRequest request)
    {
        var oldSubmission = await _unitOfWork.Submissions.GetByIdAsync(submissionId);
        if (oldSubmission == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        _mapper.Map(request, oldSubmission);

        var edit = await _unitOfWork.Submissions.UpdateAsync(oldSubmission);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }

    public async Task<SieveResponse> FindAsync(SieveModel model)
    {
        if (model?.Page < 0 || model?.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest);

        var sortedSubs = _sieveProcessor.Apply(model, _unitOfWork.Submissions.Set);
        if (sortedSubs == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

        var mappedSubs = _mapper.Map<List<SubmissionDetailsResponse>>(sortedSubs);

        return new(rows: mappedSubs,
            index: model?.Page,
            total: await _unitOfWork.Ideas.CountAsync());
    }

    public async Task<SubmissionDetailsResponse> FindByIdAsync(string submissionId)
    {
        var submission = await _unitOfWork.Submissions.GetByIdAsync(submissionId);
        return _mapper.Map<SubmissionDetailsResponse>(submission);
    }

    public async Task RemoveAsync(string submissionId)
    {
        var subExists = await _unitOfWork.Submissions.GetByIdAsync(submissionId);
        if (subExists == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var delete = await _unitOfWork.Submissions.DeleteAsync(submissionId);
        if (!delete.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }
}