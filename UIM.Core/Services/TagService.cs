namespace UIM.Core.Services;

public class TagService : Service, ITagService
{
    public TagService(IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork)
        : base(mapper, sieveProcessor, unitOfWork)
    {
    }

    public async Task CreateAsync(string name)
    {
        if (await _unitOfWork.Tags.GetByNameAsync(name) != null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var add = await _unitOfWork.Tags.AddAsync(new Tag(name));
        if (!add.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public async Task EditAsync(UpdateTagRequest request)
    {
        var tag = await _unitOfWork.Tags.GetByNameAsync(request.OldName);
        if (tag == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        tag.Name = request.NewName;

        var edit = await _unitOfWork.Tags.UpdateAsync(tag);
        if (!edit.Succeeded)
            throw new HttpException(HttpStatusCode.InternalServerError);
    }

    public IEnumerable<TagDetailsResponse> FindAll()
        => _mapper.Map<IEnumerable<TagDetailsResponse>>(_unitOfWork.Tags.Set);

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

    public async Task RemoveAsync(string name)
    {
        var tag = await _unitOfWork.Tags.GetByNameAsync(name);
        if (tag == null)
            throw new HttpException(HttpStatusCode.BadRequest);

        var delete = await _unitOfWork.Tags.DeleteAsync(tag.Id);
        if (!delete.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest);
    }
}