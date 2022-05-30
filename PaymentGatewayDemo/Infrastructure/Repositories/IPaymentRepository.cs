using PaymentGatewayDemo.Domain.Models.Payments;

namespace PaymentGatewayDemo.Infrastructure.Repositories;

/// <summary>
/// Repository of payment transactions.
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// Inserts or updates a payment entry within the repository.
    /// </summary>
    /// <param name="merchantId">The ID of the merchant.</param>
    /// <param name="paymentTransaction">The details of the payment entry.</param>
    Task UpsertPaymentTransaction(string merchantId, PaymentTransaction paymentTransaction);

    /// <summary>
    /// Retrieves a payment transaction for a given merchant.
    /// </summary>
    /// <param name="merchantId">The ID of the merchant.</param>
    /// <param name="paymentId">The ID of the payment.</param>
    /// <returns>The details of the payment if found, else <c>null</c>.</returns>
    Task<PaymentTransaction?> GetPaymentTransaction(string merchantId, string paymentId);

    // TODO: GetPayments for a specific time frame
    // This will require querying the dated entities using a row key filter
    // And then batch querying the returned payment IDs.
}
