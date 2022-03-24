namespace UIM.Core.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendAuthInfoEmailAsync(AppUser receiver, string passwordResetToken, string receiverPassword, string senderFullName, string senderTitle);
}