namespace PaymentGatewayDemo.Domain.Models.PaymentCard;

/// <summary>
/// Security details supplied with the card.
/// </summary>
public class SecurityDetails
{
    // TODO: This should be abstracted so that we can use different forms of security details as well as CVV, e.g. PIN.

    public SecurityDetails(string value)
    {
        this.Value = value;
    }

    /// <summary>
    /// Security value to validate the payment card.
    /// </summary>
    public string Value { get; private init; }
}
