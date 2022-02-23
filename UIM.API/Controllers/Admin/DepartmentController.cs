using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UIM.BAL.Services.Interfaces;
using UIM.Common;
using UIM.Common.ResponseMessages;
using UIM.Model.Dtos.Common;

namespace UIM.API.Controllers
{
    [ApiController]
    [Route("api/department-management")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost("{department}")]
        public async Task<IActionResult> Create([FromBody] string department)
        {
            if (department == null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            await _departmentService.AddAsync(department);

            return Ok(new GenericResponse());
        }
    }
}