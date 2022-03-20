namespace UIM.Core.Common;

[ApiController]
public abstract class UimController<TService> : ControllerBase
{
    protected TService _service;

    protected UimController(TService service) => _service = service;

    protected IActionResult ResponseResult(
        bool succeeded = true,
        string message = SuccessResponseMessages.RequestSucceeded,
        object? result = null)
    {
        return Ok(new CoreResponse(succeeded, message, result));
    }

    protected IActionResult ResponseResult(
        object result,
        bool succeeded = true,
        string message = SuccessResponseMessages.RequestSucceeded)
    {
        return Ok(new CoreResponse(succeeded, message, result));
    }
}