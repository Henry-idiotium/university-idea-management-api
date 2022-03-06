using Microsoft.AspNetCore.Mvc;
using UIM.Core.Services.Interfaces;
using UIM.Core.Common;
using UIM.Core.Models.Dtos.Category;

namespace UIM.Core.Controllers
{
    [ApiController]
    [Route("api/category-management")]
    public class CategoryController : UimController<ICategoryService, int,
        CreateCategoryRequest,
        UpdateCategoryRequest,
        CategoryDetailsResponse>
    {
        public CategoryController(ICategoryService service) : base(service)
        {

        }
    }
}