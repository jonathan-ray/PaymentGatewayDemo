using PaymentGatewayDemo.Application.Facades;

namespace PaymentGatewayDemo.Application.Factories;

/// <summary>
/// Factory class for creating acquiring bank services.
/// </summary>
public interface IAcquiringBankServiceFacadeFactory
{
    /// <summary>
    /// Retrieves the correct acquiring bank service for the given merchant.
    /// </summary>
    /// <param name="merchantId">The merchant's ID.</param>
    /// <returns>The merchants acquiring bank's service.</returns>
    Task<IAcquiringBankServiceFacade> GetAcquiringBankForMerchant(string merchantId);
}
