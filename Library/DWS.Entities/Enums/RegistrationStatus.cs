
namespace DWS.Entities.Enums;

// Defines the possible statuses of a product registration.
public enum RegistrationStatus
{
    // Registration is waiting for admin approval.
    Pending = 1,

    // Registration has been approved by the admin.
    Approved = 2,

    // Registration has been rejected by the admin.
    Rejected = 3
}
