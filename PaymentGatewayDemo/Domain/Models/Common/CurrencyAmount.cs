namespace PaymentGatewayDemo.Domain.Models.PaymentCard;

public class CurrencyAmount
{
    /// <summary>
    /// Currency of the amount.
    /// </summary>
    public CurrencyId Currency { get; init; }

    /// <summary>
    /// The value of the amount.
    /// </summary>
    public uint Value { get; init; }
}
