using PaymentGatewayDemo.Domain.Models.AcquiringBank;

namespace PaymentGatewayDemo.Domain.Services;

/// <summary>
/// Simulated version of an acquiring bank service.
/// In production, we would need to address all security concerns of sending this data over the wire.
/// </summary>
public class SimulatedAcquiringBankService : IAcquiringBankPaymentProcessingService<ProcessPaymentRequestModel, ProcessPaymentResponseModel>
{
    public Task<ProcessPaymentResponseModel> ProcessPayment(ProcessPaymentRequestModel request)
    {
        return Task.FromResult(new ProcessPaymentResponseModel
        {
            ReceiptId = Guid.NewGuid().ToString(),
            ProcessedOn = DateTime.UtcNow,
            Status = ProcessPaymentStatusModel.Success
        });
    }
}
