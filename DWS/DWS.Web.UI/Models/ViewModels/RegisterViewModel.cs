
using System.ComponentModel.DataAnnotations;

namespace DWS.Web.UI.Models.ViewModels;

// Holds the information submitted through the customer registration form.
public class RegisterViewModel
{
    // Stores the full name of the customer.
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    // Stores the customer's email address.
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    // Stores the customer's phone number.
    [Phone(ErrorMessage = "Enter a valid phone number.")]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    // Stores the password chosen by the customer.
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8,
        ErrorMessage = "Password must be at least 8 characters long.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    // Confirms that the customer entered the intended password.
    [Required(ErrorMessage = "Please confirm your password.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password),
        ErrorMessage = "Password and confirm password do not match.")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
