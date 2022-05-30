namespace PaymentGatewayDemo.Infrastructure.Repositories.Entities;

public class PaymentTransactionEntity : ITableEntity
{
    public string PartitionKey => this.MerchantId;

    public string RowKey => GetRowKey(RequestId);

    public string MerchantId { get; init; }

    public string RequestId { get; init; }

    public DateTime RequestedOn { get; init; }

    public string CardNumber { get; init; }

    public uint CardExpiryMonth { get; init; }

    public uint CardExpiryYear { get; init; }

    public string SecurityValue { get; init; }

    public int Currency { get; init; }

    public uint Amount { get; init; }

    public string? ReceiptId { get; init; }

    public DateTime? ProcessedOn { get; init; }

    public int PaymentStatus { get; init; }

    public static string GetRowKey(string paymentId) => $"PaymentTransaction-{paymentId}";

    // TODO: Enter other details here
}
