using PaymentGatewayDemo.Infrastructure.Repositories.Entities;

namespace PaymentGatewayDemo.Infrastructure.Repositories;

/// <summary>
/// Interface for a generic NoSQL storage solution.
/// </summary>
public interface IRepository
{
    /// <summary>
    /// Queries for an entity from a specific partition within the repository.
    /// </summary>
    /// <typeparam name="TTableEntity">Expected type of table entity.</typeparam>
    /// <param name="partitionKey">The partition key.</param>
    /// <param name="rowKey">The row key within this partition.</param>
    /// <returns>The table entity if found, else <c>null</c>.</returns>
    Task<TTableEntity?> Query<TTableEntity>(string partitionKey, string rowKey) where TTableEntity : class, ITableEntity;

    /// <summary>
    /// Inserts or updates a table entity into the repository.
    /// </summary>
    /// <param name="tableEntity">The table entity to upsert.</param>
    Task Upsert(ITableEntity tableEntity);
}
