namespace PaymentGatewayDemo.Infrastructure.Exceptions
{
    public class MerchantNotFoundException : Exception
    {
        public MerchantNotFoundException(string merchantId)
        {
            this.MerchantId = merchantId;
        }

        private string MerchantId { get; set; }

        public override string Message => $"Unable to find a merchant with the ID '{this.MerchantId}'.";
    }
}
