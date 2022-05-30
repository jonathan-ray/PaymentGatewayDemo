namespace PaymentGatewayDemo.Domain.Models.PaymentCard;

public class PaymentCardModel
{
    /// <summary>
    /// The payment card's number.
    /// </summary>
    public string CardNumber { get; init; }

    /// <summary>
    /// The expiration month of the card, expressed in its integer format.
    /// </summary>
    public uint ExpiryMonth { get; init; }

    /// <summary>
    /// The expiration year of the card.
    /// </summary>
    public uint ExpiryYear { get; init; }

    /// <summary>
    /// The CVV number.
    /// </summary>
    public string CVV { get; init; }
}
