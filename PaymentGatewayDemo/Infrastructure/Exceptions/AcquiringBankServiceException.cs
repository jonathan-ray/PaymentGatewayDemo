namespace PaymentGatewayDemo.Infrastructure.Exceptions
{
    public class AcquiringBankServiceException : Exception
    {
        public AcquiringBankServiceException(Exception innerException)
            : base($"Exception occurred whilst calling the acquiring bank service. Details: {innerException.Message}", innerException)
        {
        }
    }
}
