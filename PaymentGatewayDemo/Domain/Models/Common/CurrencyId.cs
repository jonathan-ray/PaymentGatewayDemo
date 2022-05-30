namespace PaymentGatewayDemo.Domain.Models.PaymentCard;

/// <summary>
/// Supported Currency
/// </summary>
/// <remarks>
/// If we start doing operations that differ based on currency, potentially expand this by creating a class for each currency.
/// </remarks>
public enum CurrencyId
{
    Eur,
    Gbp,
    Cad,
    Usd,
    Brl,
    // TODO: exhaustive list of currencies supported
}
