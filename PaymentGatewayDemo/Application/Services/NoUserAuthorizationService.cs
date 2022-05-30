namespace PaymentGatewayDemo.Application.Services
{
    public class NoUserAuthorizationService : IUserAuthorizationService
    {
        public Task VerifyAuthorization(HttpRequest request, string merchantId)
        {
            // For testing purposes.
            return Task.CompletedTask;
        }
    }
}
