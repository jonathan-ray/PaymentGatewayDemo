namespace PaymentGatewayDemo.Domain.Models.PaymentCard;

public class CurrencyAmountModel
{
    /// <summary>
    /// The 3-character currency code.
    /// </summary>
    public string Currency { get; init; }

    /// <summary>
    /// The value of the amount.
    /// </summary>
    public uint Value { get; init; }
}
