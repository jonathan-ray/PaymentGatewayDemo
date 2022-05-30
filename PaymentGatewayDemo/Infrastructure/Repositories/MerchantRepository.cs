using PaymentGatewayDemo.Domain.Models.Merchant;
using PaymentGatewayDemo.Infrastructure.Repositories.Entities;
using PaymentGatewayDemo.Infrastructure.Repositories.Transformers;

namespace PaymentGatewayDemo.Infrastructure.Repositories;

public class MerchantRepository : IMerchantRepository
{
    private readonly IRepository repository;
    private readonly IMerchantDetailsTransformer merchantDetailsTransformer;

    public MerchantRepository(IRepository repository, IMerchantDetailsTransformer merchantDetailsTransformer)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.merchantDetailsTransformer = merchantDetailsTransformer ?? throw new ArgumentNullException(nameof(merchantDetailsTransformer));
    }

    public async Task<MerchantDetails?> GetMerchantDetails(string merchantId)
    {
        var entity = await this.repository.Query<MerchantDetailsEntity>(merchantId, MerchantDetailsEntity.GetRowKey());

        if (entity is null)
        {
            // TODO: Add logging here.
            // Return null here as it is not inherently an issue for a repository to not have an entity.
            return null;
        }

        return this.merchantDetailsTransformer.CreateModel(entity);
    }
}
