namespace PaymentGatewayDemo.Domain.Models.Payments;

public class PaymentReceiptModel
{
    /// <summary>
    /// The details specific to the request that was made.
    /// </summary>
    public PaymentRequestModel Request { get; init; }

    /// <summary>
    /// The completed transaction's receipt ID (if exists).
    /// </summary>
    public string? BankReceiptId { get; init; }

    /// <summary>
    /// When the completed transaction was processed (if processed at all).
    /// </summary>
    public DateTime? ProcessedOn { get; init; }

    /// <summary>
    /// The status of the payment.
    /// </summary>
    public string Status { get; init; }
}
