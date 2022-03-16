namespace UIM.Core.Controllers;

[ApiController]
[Route("api/category-management")]
public class CategoryController : AdminController<ICategoryService,
    CreateCategoryRequest,
    UpdateCategoryRequest,
    CategoryDetailsResponse>
{
    public CategoryController(ICategoryService service) : base(service)
    {

    }

    [HttpGet("categories")]
    public override async Task<IActionResult> Get([FromQuery] SieveModel request) => await base.Get(request);
}