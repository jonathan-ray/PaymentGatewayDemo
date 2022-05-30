namespace PaymentGatewayDemo.Infrastructure.Exceptions
{
    public class IdempotentPaymentTransactionException : Exception
    {
        public IdempotentPaymentTransactionException(string merchantId, string paymentRequestId)
        {
            this.MerchantId = merchantId;
            this.PaymentRequestId = paymentRequestId;
        }

        private string MerchantId { get; set; }

        private string PaymentRequestId { get; set; }

        public override string Message => $"A payment transaction with request ID '{this.PaymentRequestId}' has already been made by merchant '{this.MerchantId}'.";
    }
}
