using PaymentGatewayDemo.Domain.Models.Payments;

namespace PaymentGatewayDemo.Domain.Transformers;

public interface IPaymentModelTransformer
{
    /// <summary>
    /// Creates a transaction from a payment request.
    /// </summary>
    /// <param name="paymentRequest">The payment request.</param>
    /// <returns>The payment transaction.</returns>
    PaymentTransaction CreateTransactionFromRequest(PaymentTransactionModel<PaymentRequestModel> paymentRequest);

    /// <summary>
    /// Create a receipt from the payment transaction result.
    /// </summary>
    /// <param name="paymentResult">The payment transaction result.</param>
    /// <returns>The payment receipt.</returns>
    PaymentReceiptModel CreateReceiptFromResult(PaymentRequestDetails request, PaymentTransactionResult paymentResult);

    /// <summary>
    /// Create a transaction receipt model from a payment transaction.
    /// </summary>
    /// <param name="paymentTransaction">The payment transaction.</param>
    /// <returns>The transaction receipt.</returns>
    PaymentTransactionModel<PaymentReceiptModel> CreateTransactionResponseFromTransaction(PaymentTransaction paymentTransaction);
}
