using PaymentGatewayDemo.Domain.Models.Merchant;
using PaymentGatewayDemo.Infrastructure.Repositories.Entities;

namespace PaymentGatewayDemo.Infrastructure.Repositories.Transformers;

/// <summary>
/// Transformation logic for internal models to repository entities for merchant details.
/// </summary>
public interface IMerchantDetailsTransformer
{
    /// <summary>
    /// Transforms the repository entity for a merchant into the internal model equivalent.
    /// </summary>
    /// <param name="entity">The merchant repository entity.</param>
    /// <returns>The merchant internal model equivalent.</returns>
    MerchantDetails CreateModel(MerchantDetailsEntity entity);

    /// <summary>
    /// Transform the internal model for a merchant into the repository entity equivalent.
    /// </summary>
    /// <param name="model">The merchant internal model.</param>
    /// <returns>The merchant repository entity equivalent.</returns>
    MerchantDetailsEntity CreateEntity(MerchantDetails model);
}
