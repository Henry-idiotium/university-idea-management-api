namespace UIM.Core.ResponseMessages;

public static class ErrorResponseMessages
{
    public const string BadRequest = "Invalid request";
    public const string Forbidden = "Request rejected";
    public const string SentEmailFailed = "Could not send email to user";
    public const string Unauthorized = "Unauthorized to access this resource";
    public const string UnexpectedError = "Something went wrong";
}