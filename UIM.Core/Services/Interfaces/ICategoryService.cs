namespace UIM.Core.Services.Interfaces;

public interface ICategoryService
    : IService<
        CreateCategoryRequest,
        UpdateCategoryRequest,
        CategoryDetailsResponse>
{

}