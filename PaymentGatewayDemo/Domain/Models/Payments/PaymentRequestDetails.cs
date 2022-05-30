namespace PaymentGatewayDemo.Domain.Models.Payments
{
    public class PaymentRequestDetails
    {
        /// <summary>
        /// The payment request ID.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// When the payment was requested.
        /// </summary>
        public DateTime RequestedOn { get; init; }
    }
}
