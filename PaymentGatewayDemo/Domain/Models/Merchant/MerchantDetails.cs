namespace PaymentGatewayDemo.Domain.Models.Merchant;

/// <summary>
/// Internal representaiton of a merchant's information.
/// </summary>
public class MerchantDetails
{
    /// <summary>
    /// ID of the Merchant.
    /// </summary>
    public string MerchantId { get; init; }

    /// <summary>
    /// The key to use when calling our APIs to help protect against others calling the data.
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// Unique ID of the acquiring bank this merchant uses.
    /// </summary>
    public Guid AcquiringBank { get; init; }

    /// <summary>
    ///  The ID of the merchant with the acquiring bank.
    /// </summary>
    public string MerchantBankId { get; init; }
}
