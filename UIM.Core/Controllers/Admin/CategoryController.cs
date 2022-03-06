using Microsoft.AspNetCore.Mvc;
using UIM.Core.Services.Interfaces;
using UIM.Core.Common;
using UIM.Core.Models.Dtos.Category;

namespace UIM.Core.Controllers
{
    [Route("api/department-management")]
    [ApiController]
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