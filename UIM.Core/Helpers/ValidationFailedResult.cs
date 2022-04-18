namespace UIM.Core.Helpers;

public class ValidationFailedResult : ObjectResult
{
    public ValidationFailedResult()
        : base(new CoreResponse(false, ErrorResponseMessages.BadRequest))
    {
        StatusCode = StatusCodes.Status400BadRequest;
    }
}
