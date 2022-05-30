using PaymentGatewayDemo.Domain.Models.Merchant;

namespace PaymentGatewayDemo.Infrastructure.Repositories;

/// <summary>
/// Repository of data based on a given merchant.
/// </summary>
public interface IMerchantRepository
{
    /// <summary>
    /// Retrieves the details of a given merchant.
    /// </summary>
    /// <param name="merchantId">The merchant's ID.</param>
    /// <returns>The merchant's details if found, else <c>null</c>.</returns>
    Task<MerchantDetails?> GetMerchantDetails(string merchantId);

    // TODO: Add Merchant Details to repository.
}
