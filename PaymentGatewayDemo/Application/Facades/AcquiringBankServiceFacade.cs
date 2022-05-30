using PaymentGatewayDemo.Domain.Models.AcquiringBank;
using PaymentGatewayDemo.Domain.Models.Payments;
using PaymentGatewayDemo.Domain.Services;
using PaymentGatewayDemo.Domain.Transformers;
using PaymentGatewayDemo.Infrastructure.Exceptions;

namespace PaymentGatewayDemo.Application.Facades;

/// <summary>
/// A façade service for a specific acquiring bank.
/// </summary>
public class AcquiringBankServiceFacade : IAcquiringBankServiceFacade
{
    // TODO: This would be better suited to a generator pattern construction, especially when we have more than one acquiring bank.

    private readonly string merchantBankId;
    private readonly IAcquiringBankPaymentProcessingService<ProcessPaymentRequestModel, ProcessPaymentResponseModel> acquiringBankService;
    private readonly IAcquiringBankModelTransformer<ProcessPaymentRequestModel, ProcessPaymentResponseModel> modelTransformer;

    public AcquiringBankServiceFacade(
        string merchantBankId,
        IAcquiringBankPaymentProcessingService<ProcessPaymentRequestModel, ProcessPaymentResponseModel> acquiringBankService,
        IAcquiringBankModelTransformer<ProcessPaymentRequestModel, ProcessPaymentResponseModel> modelTransformer)
    {
        this.merchantBankId = merchantBankId;
        this.acquiringBankService = acquiringBankService ?? throw new ArgumentNullException(nameof(acquiringBankService));
        this.modelTransformer = modelTransformer ?? throw new ArgumentNullException(nameof(modelTransformer));
    }

    public async Task<PaymentTransactionResult> ProcessTransaction(PaymentTransaction paymentTransaction)
    {
        if (paymentTransaction is null)
        {
            throw new ArgumentNullException(nameof(paymentTransaction));
        }

        var processRequest = this.modelTransformer.CreateProcessRequestFromTransaction(this.merchantBankId, paymentTransaction);

        ProcessPaymentResponseModel processResponse;
        try
        {
            processResponse = await this.acquiringBankService.ProcessPayment(processRequest);
        }
        catch (Exception exception)
        {
            // TODO: Introduce retry logic here.
            // Determine if the failure was intermittent and potentially retry a pre-determined amount of times.
            throw new AcquiringBankServiceException(exception);
        }

        return this.modelTransformer.CreateTransactionResultFromProcessResponse(processResponse);
    }
}
