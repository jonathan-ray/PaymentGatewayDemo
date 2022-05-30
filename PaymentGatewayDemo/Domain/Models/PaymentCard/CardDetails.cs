namespace PaymentGatewayDemo.Domain.Models.PaymentCard;

public class CardDetails
{
    /// <summary>
    /// The card number.
    /// </summary>
    public CardNumber Number { get; init; }

    /// <summary>
    /// Expiration date of the card.
    /// </summary>
    public MonthlyDate ExpiryDate { get; init; }

    /// <summary>
    /// Security details associated with this card.
    /// </summary>
    public SecurityDetails SecurityDetails { get; init; }
}
