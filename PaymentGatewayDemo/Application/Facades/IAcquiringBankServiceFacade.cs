using PaymentGatewayDemo.Domain.Models.Payments;

namespace PaymentGatewayDemo.Application.Facades;

/// <summary>
/// Internal facade for acquiring bank services.
/// </summary>
public interface IAcquiringBankServiceFacade
{
    /// <summary>
    /// Processes a payment transaction with a given acquiring bank service.
    /// </summary>
    /// <param name="paymentTransaction">The payment transaction.</param>
    /// <returns>The payment transaction's result.</returns>
    Task<PaymentTransactionResult> ProcessTransaction(PaymentTransaction paymentTransaction);
}
