namespace PaymentGatewayDemo.Domain.Models.AcquiringBank;

/// <summary>
/// Example response for payment processing by an acquiring bank.
/// </summary>
/// <remarks>
/// In production, this would align with the contract of the acquiring bank's API.
/// </remarks>
public class ProcessPaymentResponseModel
{
    /// <summary>
    /// The receipt ID.
    /// </summary>
    public string ReceiptId { get; init; }

    /// <summary>
    /// Date and time the processing was complete.
    /// </summary>
    public DateTime ProcessedOn { get; init; }

    /// <summary>
    /// The status of the processing.
    /// </summary>
    public ProcessPaymentStatusModel Status { get; init; }
}
