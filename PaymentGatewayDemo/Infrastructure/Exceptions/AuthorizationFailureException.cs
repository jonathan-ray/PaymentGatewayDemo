namespace PaymentGatewayDemo.Infrastructure.Exceptions
{
    public class AuthorizationFailureException : Exception
    {
        public AuthorizationFailureException(string description)
        {
            this.Description = description;
        }

        private string Description { get; set; }

        public override string Message => $"Authorization for this call has failed: {this.Description}";
    }
}
