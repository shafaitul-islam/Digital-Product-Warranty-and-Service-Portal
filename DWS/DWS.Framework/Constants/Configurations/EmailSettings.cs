
namespace DWS.Framework.Configurations;

// Stores the Gmail SMTP configuration used by the email service.
public class EmailSettings
{
    // Defines the Gmail SMTP server address.
    public string SmtpServer { get; set; } = string.Empty;

    // Defines the SMTP server port.
    public int SmtpPort { get; set; } = 587;

    // Defines the display name of the email sender.
    public string SenderName { get; set; } = string.Empty;

    // Defines the sender's Gmail address.
    public string SenderEmail { get; set; } = string.Empty;

    // Defines the Gmail username used for SMTP authentication.
    public string Username { get; set; } = string.Empty;

    // Defines the Gmail App Password used for SMTP authentication.
    public string Password { get; set; } = string.Empty;
}
