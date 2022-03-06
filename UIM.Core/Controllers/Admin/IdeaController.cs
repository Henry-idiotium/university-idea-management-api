using Microsoft.AspNetCore.Mvc;
using UIM.Core.Common;
using UIM.Core.Models.Dtos.Idea;
using UIM.Core.Services.Interfaces;

namespace Namespace
{
    [ApiController]
    [Route("api/idea-management")]
    public class IdeaController : UimController<IIdeaService, string,
        CreateIdeaRequest,
        UpdateIdeaRequest,
        IdeaDetailsResponse>
    {
        public IdeaController(IIdeaService ideaService) : base(ideaService)
        {

        }
    }
}