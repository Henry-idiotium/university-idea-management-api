namespace UIM.Core.Helpers;

public class SnakecasingParameOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();
        else
        {
            foreach (var item in operation.Parameters)
            {
                item.Name = item.Name.ToSnakeCase();
            }
        }
    }
}