using PaymentGatewayDemo.Domain.Models.Payments;
using PaymentGatewayDemo.Infrastructure.Repositories.Entities;
using PaymentGatewayDemo.Infrastructure.Repositories.Transformers;

namespace PaymentGatewayDemo.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly IRepository repository;
    private readonly IPaymentTransactionTransformer paymentDetailsTransformer;

    public PaymentRepository(IRepository repository, IPaymentTransactionTransformer paymentDetailsTransformer)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.paymentDetailsTransformer = paymentDetailsTransformer ?? throw new ArgumentNullException(nameof(paymentDetailsTransformer));
    }

    public async Task<PaymentTransaction?> GetPaymentTransaction(string merchantId, string paymentId)
    {
        var entity = await repository.Query<PaymentTransactionEntity>(merchantId, PaymentTransactionEntity.GetRowKey(paymentId));

        if (entity is null)
        {
            // TODO: Add logging here.
            // Return null here as it is not inherently an issue for a repository to not have an entity.
            return null;
        }

        return paymentDetailsTransformer.CreateModel(entity);
    }

    public async Task UpsertPaymentTransaction(string merchantId, PaymentTransaction paymentDetails)
    {
        var detailsEntity = paymentDetailsTransformer.CreateDetailsEntity(merchantId, paymentDetails);

        // The dated entity, while not used now, is something that will be relied upon when we want to do query ranges
        // e.g. get me all payment transactions today/this month/this year.
        var datedEntity = paymentDetailsTransformer.CreateDatedEntity(merchantId, paymentDetails);

        await Task.WhenAll(
            repository.Upsert(detailsEntity),
            repository.Upsert(datedEntity));
    }
}
