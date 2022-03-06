using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using UIM.Core.Helpers;
using UIM.Core.ResponseMessages;
using UIM.Core.Services.Interfaces;
using UIM.Core.Models.Dtos;
using UIM.Core.Models.Dtos.Submission;
using UIM.Core.Common;

namespace UIM.Core.Controllers.Admin
{
    [ApiController]
    [Route("api/submission-management")]
    public class SubmissionController : UimController<ISubmissionService, string,
        CreateSubmissionRequest,
        UpdateSubmissionRequest,
        SubmissionDetailsResponse>
    {
        public SubmissionController(ISubmissionService service) : base(service)
        {
        }

        [HttpPost("[controller]/idea")]
        public async Task<IActionResult> AddIdea([FromQuery] AddIdeaRequest request)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            await _service.AddIdeaToSubmissionAsync(request);
            return Ok(new GenericResponse());
        }
    }
}