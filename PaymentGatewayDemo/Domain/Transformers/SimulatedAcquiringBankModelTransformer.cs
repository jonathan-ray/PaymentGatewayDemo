using PaymentGatewayDemo.Domain.Models.AcquiringBank;
using PaymentGatewayDemo.Domain.Models.Payments;

namespace PaymentGatewayDemo.Domain.Transformers
{
    public class SimulatedAcquiringBankModelTransformer : IAcquiringBankModelTransformer<ProcessPaymentRequestModel, ProcessPaymentResponseModel>
    {
        public ProcessPaymentRequestModel CreateProcessRequestFromTransaction(string merchantBankId, PaymentTransaction paymentTransaction)
        {
            return new ProcessPaymentRequestModel
            {
                MerchantId = merchantBankId,
                RequestId = paymentTransaction.RequestDetails.Id,
                PaymentCardNumber = paymentTransaction.CardDetails.Number.Value,
                PaymentCardExpiryMonth = paymentTransaction.CardDetails.ExpiryDate.Month,
                PaymentCardExpiryYear = paymentTransaction.CardDetails.ExpiryDate.Year,
                CvvNumber = paymentTransaction.CardDetails.SecurityDetails.Value,
                Currency = paymentTransaction.Amount.Currency.ToString(),
                Amount = paymentTransaction.Amount.Value,
            };
        }

        public PaymentTransactionResult CreateTransactionResultFromProcessResponse(ProcessPaymentResponseModel processResponse)
        {
            return new PaymentTransactionResult(
                ConvertStatus(processResponse.Status), 
                new PaymentReceiptDetails(processResponse.ReceiptId, processResponse.ProcessedOn));
        }

        private static PaymentStatus ConvertStatus(ProcessPaymentStatusModel statusModel)
        {
            return statusModel switch
            {
                ProcessPaymentStatusModel.Success => PaymentStatus.Success,
                ProcessPaymentStatusModel.ValidationFailed => PaymentStatus.ValidationFailed,
                ProcessPaymentStatusModel.AuthorizationFailed => PaymentStatus.AuthorizationFailed,
                ProcessPaymentStatusModel.PaymentDenied => PaymentStatus.PaymentDenied,
                ProcessPaymentStatusModel.IntermittentServerFailure => PaymentStatus.IntermittentServerFailure,
                ProcessPaymentStatusModel.PersistentServerFailure => PaymentStatus.PersistentServerFailure,
                _ => PaymentStatus.Unknown
            };
        }
    }
}
