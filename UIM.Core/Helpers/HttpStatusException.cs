namespace UIM.Core.Helpers;

public class HttpException : Exception
{
    public HttpException(HttpStatusCode status, string? message = null)
        : base(message ?? ResponseMessage(status))
    {
        Status = status;
    }

    static string ResponseMessage(HttpStatusCode status) =>
        status switch
        {
            HttpStatusCode.Forbidden => ErrorResponseMessages.Forbidden,
            HttpStatusCode.BadRequest => ErrorResponseMessages.BadRequest,
            HttpStatusCode.Unauthorized => ErrorResponseMessages.Unauthorized,
            HttpStatusCode.InternalServerError => ErrorResponseMessages.UnexpectedError,
            _ => ErrorResponseMessages.UnexpectedError
        };

    public HttpStatusCode Status { get; private set; }
}
