namespace PaymentGatewayDemo.Infrastructure.Repositories.Entities;

public class MerchantDetailsEntity : ITableEntity
{
    public string PartitionKey => this.MerchantId;

    public string RowKey => GetRowKey();

    /// <summary>
    /// ID of the Merchant.
    /// </summary>
    public string MerchantId { get; init; }

    /// <summary>
    /// The key to use when calling our APIs to help protect against others calling the data.
    /// </summary>
    public string ApiKey { get; init; }

    /// <summary>
    /// Unique ID of the acquiring bank this merchant uses.
    /// </summary>
    public Guid AcquiringBank { get; init; }

    /// <summary>
    /// The merchant's ID with the acquiring bank.
    /// </summary>
    public string MerchantBankId { get; init; }

    // Add other merchant-related data we need here...

    public static string GetRowKey()
    {
        return "MerchantDetails";
    }
}
