namespace PaymentGatewayDemo.Domain.Models.AcquiringBank;

/// <summary>
/// Example statuses for payment processing by an acquiring bank.
/// </summary>
/// <remarks>
/// In production, this would align with the contract of the acquiring bank's API.
/// </remarks>
public enum ProcessPaymentStatusModel
{
    Unknown,
    Success,
    ValidationFailed,
    AuthorizationFailed,
    PaymentDenied,
    IntermittentServerFailure,
    PersistentServerFailure
}
