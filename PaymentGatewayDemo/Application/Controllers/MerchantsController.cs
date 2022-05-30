using Microsoft.AspNetCore.Mvc;
using PaymentGatewayDemo.Application.Services;
using PaymentGatewayDemo.Domain.Models.Payments;
using PaymentGatewayDemo.Domain.Transformers;
using PaymentGatewayDemo.Infrastructure.Exceptions;

namespace PaymentGatewayDemo.Application.Controllers;

/// <summary>
/// Controller for handling all merchant-related APIs.
/// </summary>
[ApiController]
[Route("[controller]")]
public class MerchantsController : ControllerBase
{
    // TODO: Desired expansion would be to silo per merchant to prevent unauthorized changes between merchants.

    private readonly IUserAuthorizationService authorizationService;
    private readonly IPaymentService paymentService;
    private readonly IPaymentModelTransformer paymentModelTransformer;

    public MerchantsController(
        IUserAuthorizationService authorizationService,
        IPaymentService paymentService,
        IPaymentModelTransformer paymentModelTransformer)
    {
        this.authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        this.paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        this.paymentModelTransformer = paymentModelTransformer ?? throw new ArgumentNullException(nameof(paymentModelTransformer));
    }

    /// <summary>
    /// Retrieves full details of a payment transaction for a merchant given its initial request ID.
    /// </summary>
    /// <param name="merchantId">The Merchant's ID.</param>
    /// <param name="paymentRequestId">The initial payment's request ID.</param>
    /// <returns>The corresponding payment transaction.</returns>
    [HttpGet("{merchantId}/payments/{paymentRequestId}")]
    public async Task<PaymentTransactionModel<PaymentReceiptModel>?> GetPaymentTransaction(string merchantId, string paymentRequestId)
    {
        if (string.IsNullOrWhiteSpace(merchantId))
        {
            throw new ArgumentNullException(nameof(merchantId));
        }

        if (string.IsNullOrWhiteSpace(paymentRequestId))
        {
            throw new ArgumentNullException(nameof(paymentRequestId));
        }

        // Annotation: The payment request ID is used here in favour of the acquiring bank's receipt ID,
        // This is because not all processing transactions will potentially result in a successful transaction with receipt ID,
        // and furthermore this will prevent confusion on the contract with merchants by giving a single primary key across all calls.
        await authorizationService.VerifyAuthorization(this.Request, merchantId);
        var transactionReceipt = await this.paymentService.GetPaymentTransaction(merchantId, paymentRequestId);

        if (transactionReceipt is null)
        {
            throw new PaymentTransactionNotFoundException(merchantId, paymentRequestId);
        }

        return this.paymentModelTransformer.CreateTransactionResponseFromTransaction(transactionReceipt);
    }

    /// <summary>
    /// Attempts to process a given payment transaction request with the acquiring bank for a given merchant, and stores the results.
    /// </summary>
    /// <param name="merchantId">The Merchant's ID.</param>
    /// <param name="paymentRequest">The payment transaction request.</param>
    /// <returns>The resulting receipt of the transaction.</returns>
    [HttpPost("{merchantId}/payments")]
    public async Task<PaymentReceiptModel> ProcessPayment(string merchantId, [FromBody] PaymentTransactionModel<PaymentRequestModel> paymentRequest)
    {
        if (string.IsNullOrWhiteSpace(merchantId))
        {
            throw new ArgumentNullException(nameof(merchantId));
        }

        if (paymentRequest is null)
        {
            throw new ArgumentNullException(nameof(paymentRequest));
        }

        await authorizationService.VerifyAuthorization(this.Request, merchantId);
        var transactionRequest = this.paymentModelTransformer.CreateTransactionFromRequest(paymentRequest);
        var transactionResult = await this.paymentService.ProcessPaymentTransaction(merchantId, transactionRequest);
        return this.paymentModelTransformer.CreateReceiptFromResult(transactionRequest.RequestDetails, transactionResult);
    }
}
