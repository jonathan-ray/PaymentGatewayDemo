namespace PaymentGatewayDemo.Domain.Models.Payments;

/// <summary>
/// Collection of possible statuses to return to the merchant.
/// </summary>
public enum PaymentReceiptStatusModel
{
    Unknown,
    Pending,
    Success,
    ValidationFailed,
    AuthorizationFailed,
    PaymentDenied,
    IntermittentServerFailure,
    PersistentServerFailure
}
