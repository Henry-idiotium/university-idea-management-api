using UIM.Core.Common;
using UIM.Core.Models.Dtos.Category;

namespace UIM.Core.Services.Interfaces
{
    public interface ICategoryService
        : IService<int,
            CreateCategoryRequest,
            UpdateCategoryRequest,
            CategoryDetailsResponse>
    {

    }
}