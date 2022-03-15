namespace UIM.Core.Helpers;

public class ActionResponse : IActionResult
{
    private readonly GenericResponse _response;

    public ActionResponse(bool succeeded = true,
        string message = SuccessResponseMessages.RequestSucceeded)
    {
        _response = new GenericResponse(succeeded, message);
    }

    public ActionResponse(object result,
        bool succeeded = true,
        string message = SuccessResponseMessages.RequestSucceeded)
    {
        _response = new GenericResponse(result, succeeded, message);
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var objectResult = new ObjectResult(_response)
        {
            StatusCode = StatusCodes.Status200OK
        };
        await objectResult.ExecuteResultAsync(context);
    }
}