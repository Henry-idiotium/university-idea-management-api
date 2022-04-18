namespace UIM.Core.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendNotifyNewlyCreatedPostAsync(
        AppUser receiver,
        AppUser author,
        Idea newIdea,
        Submission submission
    );
    Task<bool> SendNotifySomeoneCommentedAsync(
        AppUser receiver,
        Idea receiverIdea,
        string commentContent
    );
    Task<bool> SendWelcomeEmailAsync(AppUser receiver, string receiverPassword);
}
