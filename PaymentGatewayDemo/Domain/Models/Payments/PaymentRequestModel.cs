namespace PaymentGatewayDemo.Domain.Models.Payments;

/// <summary>
/// Details of the payment request.
/// </summary>
public class PaymentRequestModel
{
    /// <summary>
    /// The request's ID.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// When the request was made.
    /// </summary>
    public DateTime RequestedOn { get; init; }
}
