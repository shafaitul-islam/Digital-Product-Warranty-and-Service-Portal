
using System.ComponentModel.DataAnnotations;

namespace DWS.Web.UI.Models.ViewModels;

// Holds the email address submitted for password recovery.
public class ForgotPasswordViewModel
{
    // Requires the customer to provide an email address.
    [Required(ErrorMessage = "Email address is required.")]

    // Validates that the supplied value has a valid email format.
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;
}
