namespace PaymentGatewayDemo.Domain.Models.Payments
{
    public class PaymentTransactionResult
    {
        public PaymentTransactionResult(PaymentStatus status, PaymentReceiptDetails? receipt = null)
        {
            this.Status = status;
            this.Receipt = receipt;
        }

        /// <summary>
        /// The receipt details of the payment made.
        /// </summary>
        public PaymentReceiptDetails? Receipt { get; private init; }

        /// <summary>
        /// The status of the payment.
        /// </summary>
        public PaymentStatus Status { get; private init; }
    }
}
