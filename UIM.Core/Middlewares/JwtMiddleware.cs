namespace UIM.Core.Middlewares;

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
            var userClaims = jwtService.GetClaimsPrincipal(token)?.Claims;
            if (userClaims == null)
                throw new InvalidOperationException();

            var role = userClaims.First(_ => _.Type == UimClaimTypes.Role).Value;

            context.User.AddIdentity(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(UimClaimTypes.Id, userId!),
                    new Claim(UimClaimTypes.Role, role)
                }));
        }

        await _next(context);
    }
}