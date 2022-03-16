namespace UIM.Core.Services;

public class CategoryService
    : Service<
        CreateCategoryRequest,
        UpdateCategoryRequest,
        CategoryDetailsResponse>,
    ICategoryService
{
    public CategoryService(IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork)
        : base(mapper, sieveProcessor, unitOfWork)
    {
    }

    public override async Task CreateAsync(CreateCategoryRequest request)
    {
        if (await _unitOfWork.Categories.GetByNameAsync(request.Name) != null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var added = await _unitOfWork.Categories.AddAsync(
            new Category { Name = request.Name });
        if (!added)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);
    }

    public override async Task EditAsync(string categoryId, UpdateCategoryRequest request)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
        if (category == null)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        category.Name = request.Name;
        var edited = await _unitOfWork.Categories.UpdateAsync(category);
        if (!edited)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);
    }

    public override async Task<SieveResponse> FindAsync(SieveModel model)
    {
        if (model?.Page < 0 || model?.PageSize < 1)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);

        var sortedCategories = _sieveProcessor.Apply(model, _unitOfWork.Categories.Set);
        if (sortedCategories == null)
            throw new HttpException(HttpStatusCode.InternalServerError,
                                    ErrorResponseMessages.UnexpectedError);

        var mappedCategories = _mapper.Map<List<CategoryDetailsResponse>>(sortedCategories);

        return new(rows: mappedCategories,
            index: model?.Page,
            total: await _unitOfWork.Ideas.CountAsync());
    }

    public override async Task<CategoryDetailsResponse> FindByIdAsync(string categoryId)
    {
        var category = _mapper.Map<CategoryDetailsResponse>(
            await _unitOfWork.Categories.GetByIdAsync(categoryId));
        return category;
    }

    public override async Task RemoveAsync(string categoryId)
    {
        var succeeded = await _unitOfWork.Categories.DeleteAsync(categoryId);
        if (!succeeded)
            throw new HttpException(HttpStatusCode.BadRequest,
                                    ErrorResponseMessages.BadRequest);
    }
}