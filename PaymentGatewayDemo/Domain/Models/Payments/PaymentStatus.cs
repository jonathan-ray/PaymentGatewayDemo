namespace PaymentGatewayDemo.Domain.Models.Payments;

/// <summary>
/// Internal representation of possible payment transaction statuses.
/// </summary>
public enum PaymentStatus
{
    Unknown = 0,
    Pending,
    Success,
    ValidationFailed,
    AuthorizationFailed,
    PaymentDenied,
    IntermittentServerFailure,
    PersistentServerFailure
}
