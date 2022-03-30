namespace UIM.Core.Models.Dtos;

public class CoreResponse
{
    public CoreResponse(
        bool succeeded = true,
        string message = SuccessResponseMessages.RequestSucceeded,
        object? result = null)
    {
        Result = result;
        Message = message;
        Succeeded = succeeded;
    }

    public bool Succeeded { get; }
    public string Message { get; }
    public DateTime TimeStamp { get; } = DateTime.Now;
    public object? Result { get; }
}