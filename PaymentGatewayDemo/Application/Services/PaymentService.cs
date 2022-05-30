using PaymentGatewayDemo.Application.Factories;
using PaymentGatewayDemo.Domain.Models.Payments;
using PaymentGatewayDemo.Infrastructure.Exceptions;
using PaymentGatewayDemo.Infrastructure.Repositories;

namespace PaymentGatewayDemo.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IAcquiringBankServiceFacadeFactory acquiringBankServiceFactory;
    private readonly IPaymentRepository paymentRepository;

    public PaymentService(
        IAcquiringBankServiceFacadeFactory acquiringBankServiceFactory,
        IPaymentRepository paymentRepository)
    {
        this.acquiringBankServiceFactory = acquiringBankServiceFactory ?? throw new ArgumentNullException(nameof(acquiringBankServiceFactory));
        this.paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
    }

    public async Task<PaymentTransaction?> GetPaymentTransaction(string merchantId, string paymentRequestId)
    {
        if (string.IsNullOrWhiteSpace(merchantId))
        {
            throw new ArgumentNullException(nameof(merchantId));
        }

        if (string.IsNullOrWhiteSpace(paymentRequestId))
        {
            throw new ArgumentNullException(nameof(paymentRequestId));
        }

        return await this.paymentRepository.GetPaymentTransaction(merchantId, paymentRequestId);
    }

    public async Task<PaymentTransactionResult> ProcessPaymentTransaction(string merchantId, PaymentTransaction paymentTransaction)
    {
        if (string.IsNullOrWhiteSpace(merchantId))
        {
            throw new ArgumentNullException(nameof(merchantId));
        }

        if (paymentTransaction is null)
        {
            throw new ArgumentNullException(nameof(paymentTransaction));
        }

        // TODO: Consider in a future update to handle internal errors more gracefully
        // e.g. if the acquiring bank service is not reachable, store in repository as 'pending'
        // then at this point query if there are any pending transactions and silently complete them

        var idempotentTransaction = await this.GetPaymentTransaction(merchantId, paymentTransaction.RequestDetails.Id);
        if (idempotentTransaction is not null)
        {
            // TODO: Idempotency should be based on the entire transactions being equal to one another
            // Override equality operators/implement IEquatable and compare the transactions in full here instead.
            if (paymentTransaction.RequestDetails.RequestedOn == idempotentTransaction.RequestDetails.RequestedOn)
            {
                return idempotentTransaction.Result;
            }

            throw new IdempotentPaymentTransactionException(merchantId, paymentTransaction.RequestDetails.Id);
        }

        var acquiringBank = await this.acquiringBankServiceFactory.GetAcquiringBankForMerchant(merchantId);

        var transactionResult = await acquiringBank.ProcessTransaction(paymentTransaction);

        paymentTransaction.CompleteTransaction(transactionResult);

        // TODO: Add exception handling and retry logic here if the upsert has an issue.
        // Failing that, the acquiring bank service (and facade) should have a 'rollback' method.
        await this.paymentRepository.UpsertPaymentTransaction(merchantId, paymentTransaction);

        return transactionResult;
    }
}
