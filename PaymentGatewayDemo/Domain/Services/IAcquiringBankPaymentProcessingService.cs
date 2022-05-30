namespace PaymentGatewayDemo.Domain.Services;

/// <summary>
/// Abstraction of an acquiring bank's payment processing service.
/// </summary>
/// <typeparam name="TRequest">The acquiring bank's payment processing request model.</typeparam>
/// <typeparam name="TResponse">The acquiring bank's payment processing response model.</typeparam>
public interface IAcquiringBankPaymentProcessingService<TRequest, TResponse>
{
    /// <summary>
    /// Send a request to the bank to process a given payment.
    /// </summary>
    /// <param name="request">The payment request.</param>
    /// <returns>The processing's response.</returns>
    Task<TResponse> ProcessPayment(TRequest request);
}
