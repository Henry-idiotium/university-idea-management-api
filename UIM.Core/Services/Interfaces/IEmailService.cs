namespace UIM.Core.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendAuthInfoUpdatePasswordAsync(AppUser receiver, string passwordResetToken, string receiverPassword, string senderFullName, string senderTitle);
}