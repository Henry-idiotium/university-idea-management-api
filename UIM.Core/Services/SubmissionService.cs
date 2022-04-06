namespace UIM.Core.Services;

public class SubmissionService : Service, ISubmissionService
{
    public SubmissionService(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager
    ) : base(mapper, sieveProcessor, unitOfWork, userManager) { }

    public async Task CreateAsync(CreateSubmissionRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var submission = _mapper.Map<Submission>(
            request,
            opts =>
                opts.AfterMap(
                    (src, dest) =>
                    {
                        dest.CreatedBy = user.Email;
                        dest.ModifiedBy = user.Email;
                    }
                )
        );

        var add = await _unitOfWork.Submissions.AddAsync(submission);
        if (!add.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    // TODO: Validate is_active, make it dynamic a bit

    public async Task EditAsync(UpdateSubmissionRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        var subToEdit = await _unitOfWork.Submissions.GetByIdAsync(request.Id);
        if (user == null || subToEdit == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        _mapper.Map(
            request,
            subToEdit,
            opts =>
                opts.AfterMap(
                    (src, dest) =>
                    {
                        dest.ModifiedBy = user.Email;
                        dest.ModifiedDate = DateTime.Now;
                    }
                )
        );

        var edit = await _unitOfWork.Submissions.UpdateAsync(subToEdit);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }

    public async Task<SieveResponse> FindAsync(SieveModel model)
    {
        if (model.Page < 0 || model.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest);

        var sortedSubs = _sieveProcessor.Apply(model, _unitOfWork.Submissions.Set);
        if (sortedSubs == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

        var mappedSubs = _mapper.Map<List<SubmissionDetailsResponse>>(sortedSubs);

        return new(
            rows: mappedSubs,
            index: model?.Page,
            total: await _unitOfWork.Ideas.CountAsync()
        );
    }

    public IEnumerable<SimpleSubmissionResponse> FindAll()
    {
        var subs = _unitOfWork.Submissions.Set.Where(_ => _.IsActive);
        if (subs == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

        return _mapper.Map<List<SimpleSubmissionResponse>>(subs);
    }

    public async Task<SubmissionDetailsResponse> FindByIdAsync(string submissionId)
    {
        var submission = await _unitOfWork.Submissions.GetByIdAsync(submissionId);
        return _mapper.Map<SubmissionDetailsResponse>(submission);
    }

    public async Task RemoveAsync(string submissionId)
    {
        var delete = await _unitOfWork.Submissions.DeleteAsync(submissionId);
        if (!delete.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }
}
