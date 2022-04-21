using SendGrid;
using SendGrid.Helpers.Mail;
using SG = UIM.Core.Helpers.EnvVars.ExternalProvider.SendGrid;

namespace UIM.Core.Services;

public class EmailService : IEmailService
{
    private readonly string _clientDomain = EnvVars.ClientDomain;
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger) => _logger = logger;

    public async Task<bool> SendWelcomeEmailAsync(AppUser receiver, string receiverPassword)
    {
        if (!EnvVars.UseEmailService)
            return true;

        var client = new SendGridClient(SG.ApiKey);
        var message = new SendGridMessage
        {
            From = new EmailAddress(SG.SenderEmail, SG.SenderName),
            TemplateId = SG.Templates.Welcome
        };

        message.AddTo(new EmailAddress(receiver.Email, receiver.FullName));

        var subject = "UIM - Online Registration Information Email";
        message.SetTemplateData(
            new
            {
                subject,
                preheader = subject,
                register_url = _clientDomain,
                receiver = new
                {
                    email = receiver.Email,
                    full_name = receiver.FullName,
                    password = receiverPassword,
                },
            }
        );

        // Disable tracking settings
        message.SetClickTracking(false, false);
        message.SetOpenTracking(false);
        message.SetGoogleAnalytics(false);
        message.SetSubscriptionTracking(false);

        var response = await client.SendEmailAsync(message);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Failed to send an email \"{userEmail}\". Error: {errorMessage}.",
                receiver.Email,
                response.Headers.Warning
            );
            return false;
        }
        return true;
    }

    public async Task<bool> SendNotifySomeoneCommentedAsync(
        AppUser receiver,
        AppUser commenter,
        Idea receiverIdea,
        string commentContent
    )
    {
        if (!EnvVars.UseEmailService)
            return true;

        var client = new SendGridClient(SG.ApiKey);
        var message = new SendGridMessage
        {
            From = new EmailAddress(SG.SenderEmail, SG.SenderName),
            TemplateId = SG.Templates.SomeoneCommented
        };

        message.AddTo(new EmailAddress(receiver.Email, receiver.FullName));

        var subject = "UIM - Someone has commented on your idea";
        message.SetTemplateData(
            new
            {
                subject,
                preheader = subject,
                receiver = new { full_name = receiver.FullName },
                commenter = new { full_name = commenter.FullName, email = commenter.Email },
                comment = new { content = commentContent },
                idea = new
                {
                    title = receiverIdea.Title,
                    link = _clientDomain
                        + $"/idea/{EncryptHelpers.EncodeBase64Url(receiverIdea.Id)}"
                }
            }
        );

        // Disable tracking settings
        message.SetClickTracking(false, false);
        message.SetOpenTracking(false);
        message.SetGoogleAnalytics(false);
        message.SetSubscriptionTracking(false);

        var response = await client.SendEmailAsync(message);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Failed to send an email \"{userEmail}\". Error: {errorMessage}.",
                receiver.Email,
                response.Headers.Warning
            );
            return false;
        }
        return true;
    }

    public async Task<bool> SendNotifyNewlyCreatedPostAsync(
        AppUser receiver,
        AppUser author,
        Idea newIdea,
        Submission submission
    )
    {
        if (!EnvVars.UseEmailService)
            return true;

        var client = new SendGridClient(SG.ApiKey);
        var message = new SendGridMessage
        {
            From = new EmailAddress(SG.SenderEmail, SG.SenderName),
            TemplateId = SG.Templates.NewPost
        };

        message.AddTo(new EmailAddress(receiver.Email, receiver.FullName));

        var subject = $"UIM - {author.FullName} Has Posted New Idea in {submission.Title}";
        message.SetTemplateData(
            new
            {
                subject,
                preheader = subject,
                receiver = new { full_name = receiver.FullName },
                author = new { full_name = author.FullName, email = author.Email },
                idea = new
                {
                    title = newIdea.Title,
                    link = _clientDomain + $"/idea/{EncryptHelpers.EncodeBase64Url(newIdea.Id)}"
                },
                submission = new
                {
                    title = submission.Title,
                    link = _clientDomain
                        + $"/submission/{EncryptHelpers.EncodeBase64Url(submission.Id)}"
                }
            }
        );

        // Disable tracking settings
        message.SetClickTracking(false, false);
        message.SetOpenTracking(false);
        message.SetGoogleAnalytics(false);
        message.SetSubscriptionTracking(false);

        var response = await client.SendEmailAsync(message);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Failed to send an email \"{userEmail}\". Error: {errorMessage}.",
                receiver.Email,
                response.Headers.Warning
            );
            return false;
        }
        return true;
    }
}
