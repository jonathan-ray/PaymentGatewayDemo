namespace PaymentGatewayDemo.Domain.Models.PaymentCard;

public class CardNumber
{
    // TODO: Strongly-typed validation
    // i.e. ensure it is 16 digits, and trim inner spaces.

    /// <summary>
    /// Card number value.
    /// </summary>
    public string Value { get; init; }
}
