namespace UIM.Core.Helpers;

public class HttpException : Exception
{
    public HttpException(HttpStatusCode status, string? message = null) : base(message)
    {
        Status = status;
    }

    public HttpStatusCode Status { get; private set; }
}