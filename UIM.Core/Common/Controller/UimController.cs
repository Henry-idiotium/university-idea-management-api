namespace UIM.Core.Common.Controller;

[ApiController]
public abstract class UimController : ControllerBase
{
    protected IActionResult ResponseResult(
        bool succeeded = true,
        string message = SuccessResponseMessages.RequestSucceeded,
        object? result = null
    )
    {
        return Ok(new CoreResponse(succeeded, message, result));
    }

    protected IActionResult ResponseResult(
        object result,
        bool succeeded = true,
        string message = SuccessResponseMessages.RequestSucceeded
    )
    {
        return Ok(new CoreResponse(succeeded, message, result));
    }
}
