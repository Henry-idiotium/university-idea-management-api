namespace UIM.Core.Helpers.Attributes;

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
        var userId = userClaims.FirstOrDefault(_ => _.Type == UimClaimTypes.Id)?.Value;
        var role = userClaims.FirstOrDefault(_ => _.Type == UimClaimTypes.Role)?.Value;

        if (userId == null || (_roles.Any() && !_roles.Contains(role!)))
        {
            context.Result = new JsonResult(new CoreResponse
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