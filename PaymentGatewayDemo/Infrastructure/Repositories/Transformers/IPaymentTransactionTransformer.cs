using PaymentGatewayDemo.Domain.Models.Payments;
using PaymentGatewayDemo.Infrastructure.Repositories.Entities;

namespace PaymentGatewayDemo.Infrastructure.Repositories.Transformers;

/// <summary>
/// Transformation logic for internal models to repository entities for payment details.
/// </summary>
public interface IPaymentTransactionTransformer
{
    /// <summary>
    /// Transforms the repository entity for a payment entry into the internal model equivalent.
    /// </summary>
    /// <param name="entity">The payment entry repository entity.</param>
    /// <returns>The payment entry internal model equivalent.</returns>
    PaymentTransaction CreateModel(PaymentTransactionEntity entity);

    /// <summary>
    /// Transform the internal model for a payment entry into the repository entity equivalent.
    /// </summary>
    /// <param name="model">The payment entry internal model.</param>
    /// <returns>The payment entry repository entity equivalent.</returns>
    PaymentTransactionEntity CreateDetailsEntity(string merchantId, PaymentTransaction model);

    /// <summary>
    /// Transforms the internal model for a payment entry into a dated repository entity equivalent for range queries.
    /// </summary>
    /// <param name="model">The payment entry internal model.</param>
    /// <returns>The payment entry dated repository entity equivalent.</returns>
    DatedPaymentEntity CreateDatedEntity(string merchantId, PaymentTransaction model);
}
