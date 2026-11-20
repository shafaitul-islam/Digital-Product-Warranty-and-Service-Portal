
using System.Threading.Tasks;

namespace DWS.Facade.Interfaces;

// Defines the contract for sending application emails.
public interface IEmailService
{
    // Sends an email to the specified recipient.
    Task SendEmailAsync(
        string recipientEmail,
        string subject,
        string message);
}
