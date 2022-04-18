namespace UIM.Core.Services;

public class TagService : Service, ITagService
{
    public TagService(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager
    ) : base(mapper, sieveProcessor, unitOfWork, userManager) { }

    public async Task CreateAsync(CreateTagRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null || await _unitOfWork.Tags.GetByNameAsync(request.Name) != null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var tag = _mapper.Map<Tag>(
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

        var add = await _unitOfWork.Tags.AddAsync(tag);
        if (!add.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task EditAsync(UpdateTagRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        var tagToEdit = await _unitOfWork.Tags.GetByIdAsync(request.Id);
        if (user == null || tagToEdit == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        _mapper.Map(
            request,
            tagToEdit,
            opts => opts.AfterMap((src, dest) => dest.ModifiedBy = user.Email)
        );

        var edit = await _unitOfWork.Tags.UpdateAsync(tagToEdit);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public IEnumerable<SimpleTagResponse> FindAll() =>
        _mapper.Map<IEnumerable<SimpleTagResponse>>(_unitOfWork.Tags.Set);

    public async Task<SieveResponse> FindAsync(SieveModel model)
    {
        var sorted = _sieveProcessor.Apply(model, _unitOfWork.Tags.Set);
        if (sorted == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

        var mappedTag = _mapper.Map<IEnumerable<TagDetailsResponse>>(sorted);
        return new(
            rows: mappedTag,
            index: model?.Page ?? 1,
            total: await _unitOfWork.Tags.CountAsync()
        );
    }

    public async Task<TagDetailsResponse> FindByIdAsync(string tagId)
    {
        var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
        var response = _mapper.Map<TagDetailsResponse>(tag);
        return response;
    }

    public async Task<TagDetailsResponse> FindByNameAsync(string name)
    {
        var tag = await _unitOfWork.Tags.GetByNameAsync(name);
        var response = _mapper.Map<TagDetailsResponse>(tag);
        return response;
    }

    public async Task RemoveAsync(string entityId)
    {
        var delete = await _unitOfWork.Tags.DeleteAsync(entityId);
        if (!delete.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }
}
