namespace PaymentGatewayDemo.Infrastructure.Repositories.Entities;

/// <summary>
/// Abstracted stub of a table storage entity
/// </summary>
public interface ITableEntity
{
    public string PartitionKey { get; }

    public string RowKey { get; }
}
