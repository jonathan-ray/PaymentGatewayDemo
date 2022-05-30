using PaymentGatewayDemo.Domain.Models.Payments;

namespace PaymentGatewayDemo.Domain.Transformers;

public interface IAcquiringBankModelTransformer<TRequest, TResponse>
{
    /// <summary>
    /// Creates an acquiring bank's request from a payment transaction.
    /// </summary>
    /// <param name="merchantBankId">The merchant's ID with the bank.</param>
    /// <param name="paymentTransaction">The payment transaction.</param>
    /// <returns>The acquiring bank's request object.</returns>
    TRequest CreateProcessRequestFromTransaction(string merchantBankId, PaymentTransaction paymentTransaction);

    /// <summary>
    /// Creates a transaction result from an acquiring bank's response.
    /// </summary>
    /// <param name="processResponse">The acquiring bank's response object.</param>
    /// <returns>The transaction result.</returns>
    PaymentTransactionResult CreateTransactionResultFromProcessResponse(TResponse processResponse);
}
