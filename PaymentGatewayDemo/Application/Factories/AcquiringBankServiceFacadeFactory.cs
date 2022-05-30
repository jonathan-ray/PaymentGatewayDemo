using PaymentGatewayDemo.Application.Facades;
using PaymentGatewayDemo.Domain.Models.AcquiringBank;
using PaymentGatewayDemo.Domain.Services;
using PaymentGatewayDemo.Domain.Transformers;

namespace PaymentGatewayDemo.Application.Factories;

public class AcquiringBankServiceFacadeFactory : IAcquiringBankServiceFacadeFactory
{
    private readonly IAcquiringBankPaymentProcessingService<ProcessPaymentRequestModel, ProcessPaymentResponseModel> acquiringBankService;
    private readonly IAcquiringBankModelTransformer<ProcessPaymentRequestModel, ProcessPaymentResponseModel> modelTransformer;

    public AcquiringBankServiceFacadeFactory(
        IAcquiringBankPaymentProcessingService<ProcessPaymentRequestModel, ProcessPaymentResponseModel> acquiringBankService,
        IAcquiringBankModelTransformer<ProcessPaymentRequestModel, ProcessPaymentResponseModel> modelTransformer)
    {
        this.acquiringBankService = acquiringBankService ?? throw new ArgumentNullException(nameof(acquiringBankService));
        this.modelTransformer = modelTransformer ?? throw new ArgumentNullException(nameof(modelTransformer));
    }


    public Task<IAcquiringBankServiceFacade> GetAcquiringBankForMerchant(string merchantId)
    {
        // TODO: Call into the merchant repository to retrieve merchant details, including the ID of the acquiring bank
        // Create a switch statement on the ID to select the appropriate acquiring bank service that we would have injected into the above constructor.

        // Additionally, from the repository, get the merchant's bank ID.
        var merchantBankId = $"{merchantId}-Bank-ID";

        return Task.FromResult<IAcquiringBankServiceFacade>(new AcquiringBankServiceFacade(merchantBankId, acquiringBankService, modelTransformer));
    }
}
