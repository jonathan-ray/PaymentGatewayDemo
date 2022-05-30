namespace PaymentGatewayDemo.Domain.Models.AcquiringBank
{
    /// <summary>
    /// Example request for payment processing by an acquiring bank.
    /// </summary>
    /// <remarks>
    /// In production, this would align with the contract of the acquiring bank's API.
    /// </remarks>
    public class ProcessPaymentRequestModel
    {
        /// <summary>
        /// The merchant's ID with the acquiring bank.
        /// </summary>
        public string MerchantId { get; init; }

        /// <summary>
        /// The request ID supplied by the merchant.
        /// </summary>
        public string RequestId { get; init; }

        /// <summary>
        /// The payment card number.
        /// </summary>
        public string PaymentCardNumber { get; init; }

        /// <summary>
        /// The payment card expiry month.
        /// </summary>
        public uint PaymentCardExpiryMonth { get; init; }

        /// <summary>
        /// The payment card expiry year.
        /// </summary>
        public uint PaymentCardExpiryYear { get; init; }

        /// <summary>
        /// The payment card CVV number.
        /// </summary>
        public string CvvNumber { get; init; }

        /// <summary>
        /// The currency of the payment.
        /// </summary>
        public string Currency { get; init; }

        /// <summary>
        /// The amount of the payment.
        /// </summary>
        public uint Amount { get; init; }
    }
}
