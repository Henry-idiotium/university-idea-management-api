namespace UIM.Core.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendWelcomeEmailAsync(
        AppUser receiver,
        string receiverPassword,
        string senderFullName,
        string senderTitle
    );
}
