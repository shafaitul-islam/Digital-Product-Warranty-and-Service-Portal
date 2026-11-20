
using System.Net;
using System.Net.Mail;
using DWS.Facade.Interfaces;
using DWS.Framework.Configurations;
using Microsoft.Extensions.Options;

namespace DWS.Facade.Services;

// Implements the email service using Gmail SMTP.
public class EmailService : IEmailService
{
    // Stores the Gmail SMTP configuration.
    private readonly EmailSettings _emailSettings;

    // Receives email configuration through dependency injection.
    public EmailService(IOptions<EmailSettings> emailOptions)
    {
        _emailSettings = emailOptions.Value;
    }

    // Sends an email to the specified recipient asynchronously.
    public async Task SendEmailAsync(
        string recipientEmail,
        string subject,
        string message)
    {
        // Creates the sender's email address and display name.
        var sender = new MailAddress(
            _emailSettings.SenderEmail,
            _emailSettings.SenderName);

        // Creates the recipient's email address.
        var recipient = new MailAddress(recipientEmail);

        // Creates the email message.
        using var mailMessage = new MailMessage(sender, recipient)
        {
            Subject = subject,

            // Allows HTML content in password reset emails.
            Body = message,

            IsBodyHtml = true
        };

        // Configures the Gmail SMTP client.
        using var smtpClient = new SmtpClient(
            _emailSettings.SmtpServer,
            _emailSettings.SmtpPort)
        {
            // Enables secure SMTP communication using STARTTLS.
            EnableSsl = true,

            // Provides Gmail authentication credentials.
            Credentials = new NetworkCredential(
                _emailSettings.Username,
                _emailSettings.Password),

            // Uses the configured SMTP network timeout.
            Timeout = 30000
        };

        // Sends the email without blocking the application thread.
        await smtpClient.SendMailAsync(mailMessage);
    }
}
