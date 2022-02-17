using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UIM.Model.Entities;

namespace UIM.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {
        }

        /* [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.SelectMany(k => k.Value.Errors));

            var authenticateResponse = await _accountService.LoginAsync(model);

            return Ok(new GenericResponse(authenticateResponse, SuccessResponseMessages.Generic));
        } */
    }
}