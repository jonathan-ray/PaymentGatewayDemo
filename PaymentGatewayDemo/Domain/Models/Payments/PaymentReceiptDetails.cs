namespace PaymentGatewayDemo.Domain.Models.Payments
{
    /// <summary>
    /// Collection of receipt details not related to the status of a transaction.
    /// </summary>
    public class PaymentReceiptDetails
    {
        public PaymentReceiptDetails(string receiptId, DateTime processedOn)
        {
            this.ReceiptId = receiptId;
            this.ProcessedOn = processedOn;
        }

        /// <summary>
        /// The receipt ID.
        /// </summary>
        public string ReceiptId { get; private init; }

        /// <summary>
        /// When the payment was processed.
        /// </summary>
        public DateTime ProcessedOn { get; private init; }
    }
}
