using PaymentGatewayDemo.Domain.Models.PaymentCard;

namespace PaymentGatewayDemo.Domain.Models.Payments;

/// <summary>
/// The full details of a payment transaction.
/// </summary>
public class PaymentTransaction
{
    /// <summary>
    /// The request details of a transaction.
    /// </summary>
    public PaymentRequestDetails RequestDetails { get; set; }

    /// <summary>
    /// The payment card details.
    /// </summary>
    public CardDetails CardDetails { get; init; }

    /// <summary>
    /// The payment amount.
    /// </summary>
    public CurrencyAmount Amount { get; init; }

    /// <summary>
    /// The result of the payment.
    /// </summary>
    public PaymentTransactionResult Result => new PaymentTransactionResult(this.Status, this.ReceiptDetails);

    /// <summary>
    /// The receipt details of a completed transaction.
    /// </summary>
    public PaymentReceiptDetails? ReceiptDetails { get; set; }

    /// <summary>
    /// The status of the transaction.
    /// </summary>
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    /// <summary>
    /// Completes the transaction with the result of the processed payment.
    /// </summary>
    /// <param name="transactionResult">The transaction result.</param>
    public void CompleteTransaction(PaymentTransactionResult transactionResult)
    {
        if (transactionResult == null)
        {
            throw new ArgumentNullException(nameof(transactionResult));
        }

        this.ReceiptDetails = transactionResult.Receipt;
        this.Status = transactionResult.Status;
    }
}
