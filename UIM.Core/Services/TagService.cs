namespace UIM.Core.Services;

public class TagService : Service, ITagService
{
    public TagService(IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork)
        : base(mapper, sieveProcessor, unitOfWork)
    {
    }

    public async Task CreateAsync(CreateTagRequest request)
    {
        if (await _unitOfWork.Tags.GetByNameAsync(request.Name) != null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var tag = _mapper.Map<Tag>(request);

        var add = await _unitOfWork.Tags.AddAsync(tag);
        if (!add.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task EditAsync(string entityId, UpdateTagRequest request)
    {
        var tag = await _unitOfWork.Tags.GetByIdAsync(entityId);
        if (tag == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        _mapper.Map(request, tag);

        var edit = await _unitOfWork.Tags.UpdateAsync(tag);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task<SieveResponse> FindAsync(SieveModel model)
    {
        if (model?.Page < 0 || model?.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest);

        var sortedList = _sieveProcessor.Apply(model, _unitOfWork.Tags.Set);
        if (sortedList == null)
            throw new HttpException(HttpStatusCode.InternalServerError);

        return new(
            index: model?.Page,
            rows: _mapper.Map<List<DepartmentDetailsResponse>>(sortedList),
            total: await _unitOfWork.Tags.CountAsync());
    }

    public async Task<TagDetailsResponse> FindByIdAsync(string tagId)
    {
        var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
        var response = _mapper.Map<TagDetailsResponse>(tag);
        return response;
    }

    public async Task RemoveAsync(string tagId)
    {
        var delete = await _unitOfWork.Tags.DeleteAsync(tagId);
        if (!delete.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }
}