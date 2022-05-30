using PaymentGatewayDemo.Domain.Models.Payments;
using PaymentGatewayDemo.Infrastructure.Exceptions;

namespace PaymentGatewayDemo.Application.Services;

/// <summary>
/// Service to handle all payment requests.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Retrieves a payment transaction for a specific merchant by its request ID.
    /// </summary>
    /// <param name="merchantId">The Merchant ID.</param>
    /// <param name="paymentRequestId">The payment request ID.</param>
    /// <returns>The payment transaction found.</returns>
    Task<PaymentTransaction?> GetPaymentTransaction(string merchantId, string paymentRequestId);

    /// <summary>
    /// Processes a payment request for a specific merchant.
    /// </summary>
    /// <param name="merchantId">The Merchant ID.</param>
    /// <param name="paymentTransaction">The payment request transaction.</param>
    /// <returns>The payment receipt transaction.</returns>
    /// <exception cref="IdempotentPaymentTransactionException">Thrown when the given transaction is reusing the request ID for a different transaction.</exception>
    Task<PaymentTransactionResult> ProcessPaymentTransaction(string merchantId, PaymentTransaction paymentTransaction);
}
