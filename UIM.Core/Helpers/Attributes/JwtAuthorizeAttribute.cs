using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UIM.Core.Models.Dtos;
using UIM.Core.ResponseMessages;

namespace UIM.Core.Helpers.Attributes
{
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Method,
        Inherited = true,
        AllowMultiple = true)]
    public class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<string> _roles;

        public JwtAuthorizeAttribute(params string[] roles)
        {
            _roles = roles ?? Array.Empty<string>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata
                .OfType<AllowAnonymousAttribute>().Any();

            if (allowAnonymous) return;

            var userClaims = context.HttpContext.User.Claims;
            var userId = userClaims.FirstOrDefault(_ => _.Type == CustomClaimTypes.Id)?.Value;
            var role = userClaims.FirstOrDefault(_ => _.Type == CustomClaimTypes.Role)?.Value;

            if (userId == null || (_roles.Any() && !_roles.Contains(role)))
            {
                context.Result = new JsonResult(new GenericResponse
                (
                    succeeded: false,
                    message: ErrorResponseMessages.Unauthorized
                ),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
}