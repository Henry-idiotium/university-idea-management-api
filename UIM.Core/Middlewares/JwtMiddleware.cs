using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using UIM.Core.Helpers;
using UIM.Core.Models.Entities;
using UIM.Core.Services.Interfaces;

namespace UIM.Core.Middlewares
{
    public static class JwtMiddlewareExt
    {
        public static void UseJwtExt(this IApplicationBuilder app) =>
            app.UseMiddleware<JwtMiddleware>();
    }

    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context,
            IJwtService jwtService,
            UserManager<AppUser> userManager)
        {
            var token = context.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();

            var userId = jwtService.Validate(token);
            var user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var userClaims = jwtService.GetClaimsPrincipal(token).Claims;
                var role = userClaims.First(_ => _.Type == CustomClaimTypes.Role).Value;

                context.User.AddIdentity(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(CustomClaimTypes.Id, userId),
                    new Claim(CustomClaimTypes.Role, role)
                }));
            }

            await _next(context);
        }
    }
}