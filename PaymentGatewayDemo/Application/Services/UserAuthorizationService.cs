using PaymentGatewayDemo.Infrastructure.Exceptions;
using PaymentGatewayDemo.Infrastructure.Repositories;

namespace PaymentGatewayDemo.Application.Services;

public class UserAuthorizationService : IUserAuthorizationService
{
    private const string ApiKeyRequestHeader = "PaymentGateway-ApiKey";

    private readonly IMerchantRepository merchantRepository;

    public UserAuthorizationService(IMerchantRepository merchantRepository)
    {
        this.merchantRepository = merchantRepository ?? throw new ArgumentNullException(nameof(merchantRepository));
    }

    public async Task VerifyAuthorization(HttpRequest request, string merchantId)
    {
        var merchant = await merchantRepository.GetMerchantDetails(merchantId);

        if (merchant is null)
        {
            throw new MerchantNotFoundException(merchantId);
        }

        var requestApiKey = request.Headers[ApiKeyRequestHeader];

        if (string.IsNullOrWhiteSpace(requestApiKey))
        {
            throw new AuthorizationFailureException("API Key is missing.");
        }

        if (merchant.ApiKey != requestApiKey)
        {
            throw new AuthorizationFailureException("API Key is not valid for this merchant.");
        }
    }
}
