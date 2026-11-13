
using System.ComponentModel.DataAnnotations;

namespace DWS.Web.UI.Models.ViewModels;

// Holds the information submitted through the customer login form.
public class LoginViewModel
{
    // Stores the email address used to log in.
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    // Stores the user's password.
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    // Allows the user to stay signed in after closing the browser.
    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}
