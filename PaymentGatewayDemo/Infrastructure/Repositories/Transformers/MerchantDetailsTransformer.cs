using PaymentGatewayDemo.Domain.Models.Merchant;
using PaymentGatewayDemo.Infrastructure.Repositories.Entities;
using PaymentGatewayDemo.Infrastructure.Security;

namespace PaymentGatewayDemo.Infrastructure.Repositories.Transformers;

public class MerchantDetailsTransformer : IMerchantDetailsTransformer
{
    private readonly IDataEncrypter dataEncrypter;

    public MerchantDetailsTransformer(IDataEncrypter dataEncrypter)
    {
        this.dataEncrypter = dataEncrypter ?? throw new ArgumentNullException(nameof(dataEncrypter));
    }

    public MerchantDetailsEntity CreateEntity(MerchantDetails model)
    {
        return new MerchantDetailsEntity
        {
            MerchantId = model.MerchantId,
            ApiKey = dataEncrypter.Encrypt(model.ApiKey),
            AcquiringBank = model.AcquiringBank,
            MerchantBankId = model.MerchantBankId
        };
    }

    public MerchantDetails CreateModel(MerchantDetailsEntity entity)
    {
        return new MerchantDetails
        {
            MerchantId = entity.MerchantId,
            ApiKey = dataEncrypter.Decrypt(entity.ApiKey),
            AcquiringBank = entity.AcquiringBank,
            MerchantBankId = entity.MerchantBankId
        };
    }
}
