
namespace DWS.Entities.Enums;

// Defines the possible statuses of a service request.
public enum ServiceRequestStatus
{
    // Request has been submitted and is waiting for admin action.
    Pending = 1,

    // Admin has assigned the request to a service staff member.
    Assigned = 2,

    // Service staff has started working on the request.
    InProgress = 3,

    // Service work has been completed.
    Completed = 4,

    // Admin has rejected the service request.
    Rejected = 5,

    // Customer or admin has cancelled the service request.
    Cancelled = 6
}
