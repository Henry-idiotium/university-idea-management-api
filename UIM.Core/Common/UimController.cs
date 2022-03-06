using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using UIM.Core.Helpers;
using UIM.Core.Models.Dtos;
using UIM.Core.ResponseMessages;

namespace UIM.Core.Common
{
    [ApiController]
    public abstract class UimController<TService, TIdentity, TCreate, TUpdate, TDetails> : ControllerBase
        where TService : IService<TIdentity, TCreate, TUpdate, TDetails>
        where TCreate : ICreateRequest
        where TUpdate : IUpdateRequest
        where TDetails : IResponse
        where TIdentity : IConvertible
    {
        protected TService _service;

        public UimController(TService service) => _service = service;
        
        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] TCreate request)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            await _service.CreateAsync(request);
            return Ok(new GenericResponse());
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> Delete(TIdentity id)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            if (id is string)
                id = (TIdentity)Convert.ChangeType(EncryptHelpers.DecodeBase64Url(id.ToString()), typeof(TIdentity));

            await _service.RemoveAsync(id);

            return Ok(new GenericResponse());
        }

        [HttpPut("[controller]/{id}")]
        public async Task<IActionResult> Edit([FromQuery] TUpdate request, TIdentity id)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            if (id is string)
                id = (TIdentity)Convert.ChangeType(EncryptHelpers.DecodeBase64Url(id.ToString()), typeof(TIdentity));

            await _service.EditAsync(id, request);
            return Ok(new GenericResponse());
        }

        [HttpGet("[controller]s")]
        public async Task<IActionResult> Get([FromQuery] SieveModel request)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var ideas = await _service.FindAsync(request);
            return Ok(new GenericResponse(ideas));
        }

        [HttpGet("[controller]/{id}")]
        public async Task<IActionResult> Get(TIdentity id)
        {
            if (!ModelState.IsValid)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            if (id is string)
                id = (TIdentity)Convert.ChangeType(EncryptHelpers.DecodeBase64Url(id.ToString()), typeof(TIdentity));

            var idea = await _service.FindByIdAsync(id);
            return Ok(new GenericResponse(idea));
        }
    }
}