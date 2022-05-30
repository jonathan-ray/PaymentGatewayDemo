namespace PaymentGatewayDemo.Infrastructure.Repositories.Entities;

public class DatedPaymentEntity : ITableEntity
{
    public string PartitionKey => this.MerchantId;

    public string RowKey => $"DatedPayment-{this.RequestedOn.ToString("yyyy-MM-dd")}-{this.RequestId}";

    public string MerchantId { get; init; }

    public DateTime RequestedOn { get; init; }

    public string RequestId { get; init; }
}
