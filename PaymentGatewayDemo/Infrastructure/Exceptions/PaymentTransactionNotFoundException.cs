namespace PaymentGatewayDemo.Infrastructure.Exceptions
{
    public class PaymentTransactionNotFoundException : Exception
    {
        public PaymentTransactionNotFoundException(string merchantId, string paymentRequestId)
        {
            this.MerchantId = merchantId;
            this.PaymentRequestId = paymentRequestId;
        }

        private string MerchantId { get; set; }

        private string PaymentRequestId { get; set; }

        public override string Message => $"Unable to find payment transaction with request ID '{this.PaymentRequestId}' by merchant '{this.MerchantId}'.";
    }
}
