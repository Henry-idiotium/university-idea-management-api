using SendGrid;
using SendGrid.Helpers.Mail;
using SG = UIM.Core.Helpers.EnvVars.ExternalProvider.SendGrid;

namespace UIM.Core.Services;

public class EmailService : IEmailService
{
    private readonly string _clientDomain = EnvVars.ClientDomain;
    private readonly string _profilePath = EnvVars.UserProfilePath;
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger) => _logger = logger;

    public async Task<bool> SendWelcomeEmailAsync(AppUser receiver,
        string receiverPassword,
        string senderFullName,
        string senderTitle)
    {
        var client = new SendGridClient(SG.ApiKey);
        var message = new SendGridMessage
        {
            From = new EmailAddress(SG.SenderEmail, SG.SenderName),
            TemplateId = SG.TemplateId
        };

        message.AddTo(new EmailAddress(receiver.Email, receiver.FullName));

        var subject = "UNIVERSITY IDEA MANAGEMENT - Online Registration Information Email";
        message.SetTemplateData(new
        {
            subject,
            preheader = subject,
            register_url = $"{_clientDomain}/{_profilePath}",
            receiver = new
            {
                email = receiver.Email,
                full_name = receiver.FullName,
                password = receiverPassword,
            },
            sender = new
            {
                full_name = senderFullName,
                title = senderTitle,
            }
        });

        // Disable tracking settings
        message.SetClickTracking(false, false);
        message.SetOpenTracking(false);
        message.SetGoogleAnalytics(false);
        message.SetSubscriptionTracking(false);

        var response = await client.SendEmailAsync(message);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to send an email \"{userEmail}\". Error: {errorMessage}.",
                receiver.Email,
                response.Headers.Warning);
            return false;
        }
        return true;
    }
}
