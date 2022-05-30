using PaymentGatewayDemo.Infrastructure.Repositories.Entities;

namespace PaymentGatewayDemo.Infrastructure.Repositories;

/// <summary>
/// Stubbed out implementation of a generic NoSQL storage solution.
/// </summary>
public class GenericRepository : IRepository
{
    private readonly IDictionary<string, IDictionary<string, ITableEntity>> internalRepository;

    public GenericRepository(IDictionary<string, IDictionary<string, ITableEntity>> internalRepository)
    {
        this.internalRepository = internalRepository ?? throw new ArgumentNullException(nameof(internalRepository));
    }

    public Task<TTableEntity?> Query<TTableEntity>(string partitionKey, string rowKey) where TTableEntity : class, ITableEntity
    {
        if (internalRepository.ContainsKey(partitionKey) && internalRepository[partitionKey].ContainsKey(rowKey))
        {
            return Task.FromResult(internalRepository[partitionKey][rowKey] as TTableEntity);
        }

        return Task.FromResult((TTableEntity?)null);
    }

    public Task Upsert(ITableEntity tableEntity)
    {
        if (internalRepository.ContainsKey(tableEntity.PartitionKey))
        {
            internalRepository[tableEntity.PartitionKey][tableEntity.RowKey] = tableEntity;
        }
        else
        {
            internalRepository[tableEntity.PartitionKey] = new Dictionary<string, ITableEntity>(StringComparer.OrdinalIgnoreCase)
            {
                { tableEntity.RowKey, tableEntity }
            };
        }

        return Task.CompletedTask;
    }
}
