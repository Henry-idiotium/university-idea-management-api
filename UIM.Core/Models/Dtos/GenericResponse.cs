namespace UIM.Core.Models.Dtos;

public class GenericResponse
{
    public GenericResponse(
        bool succeeded = true,
        string message = SuccessResponseMessages.RequestSucceeded)
    {
        Message = message;
        Succeeded = succeeded;
    }

    public GenericResponse(
        object result,
        bool succeeded = true,
        string message = SuccessResponseMessages.RequestSucceeded)
    {
        Result = result;
        Message = message;
        Succeeded = succeeded;
    }

    public bool Succeeded { get; }
    public string Message { get; }
    public DateTime TimeStamp { get; } = DateTime.UtcNow;
    public object? Result { get; }
}