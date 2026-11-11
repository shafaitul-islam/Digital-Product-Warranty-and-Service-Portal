
using System;
using Microsoft.AspNetCore.Identity;

namespace DWS.Entities.Models;

// Represents an application user, including Admin, Customer, and Service Staff.
public class ApplicationUser : IdentityUser<int>
{
    // Stores the full name of the user.
    public string FullName { get; set; } = string.Empty;

    // Indicates whether the user account is active.
    public bool IsActive { get; set; } = true;

    // Stores the date and time when the user account was created.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
