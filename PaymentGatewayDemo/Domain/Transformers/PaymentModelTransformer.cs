using PaymentGatewayDemo.Domain.Models.PaymentCard;
using PaymentGatewayDemo.Domain.Models.Payments;

namespace PaymentGatewayDemo.Domain.Transformers
{
    public class PaymentModelTransformer : IPaymentModelTransformer
    {
        public PaymentReceiptModel CreateReceiptFromResult(PaymentRequestDetails request, PaymentTransactionResult paymentResult)
        {
            return new PaymentReceiptModel
            {
                Request = new PaymentRequestModel
                {
                    Id = request.Id,
                    RequestedOn = request.RequestedOn
                },
                Status = paymentResult.Status.ToString(),
                BankReceiptId = paymentResult.Receipt?.ReceiptId,
                ProcessedOn = paymentResult.Receipt?.ProcessedOn,
            };
        }

        public PaymentTransaction CreateTransactionFromRequest(PaymentTransactionModel<PaymentRequestModel> paymentRequest)
        {
            return new PaymentTransaction
            {
                RequestDetails = new PaymentRequestDetails
                {
                    Id = paymentRequest.Details.Id,
                    RequestedOn = paymentRequest.Details.RequestedOn
                },
                CardDetails = new CardDetails
                {
                    Number = new CardNumber
                    {
                        Value = paymentRequest.PaymentCard.CardNumber
                    },
                    ExpiryDate = new MonthlyDate
                    {
                        Month = paymentRequest.PaymentCard.ExpiryMonth,
                        Year = paymentRequest.PaymentCard.ExpiryYear
                    },
                    SecurityDetails = new SecurityDetails(paymentRequest.PaymentCard.CVV)
                },
                Amount = new CurrencyAmount
                {
                    Currency = Enum.Parse<CurrencyId>(paymentRequest.Amount.Currency),
                    Value = paymentRequest.Amount.Value
                }
            };
        }

        public PaymentTransactionModel<PaymentReceiptModel> CreateTransactionResponseFromTransaction(PaymentTransaction paymentTransaction)
        {
            // TODO: Hashing values should be handled in its own class,
            // We should also salt the hashed values to help prevent identifying information.

            return new PaymentTransactionModel<PaymentReceiptModel>
            {
                Details = this.CreateReceiptFromResult(paymentTransaction.RequestDetails, paymentTransaction.Result),
                PaymentCard = new PaymentCardModel
                {
                    CardNumber = paymentTransaction.CardDetails.Number.Value.GetHashCode().ToString(),
                    ExpiryMonth = paymentTransaction.CardDetails.ExpiryDate.Month,
                    ExpiryYear = paymentTransaction.CardDetails.ExpiryDate.Year,
                    // Leaving out CVV value for privacy concerns.
                },
                Amount = new CurrencyAmountModel
                {
                    Currency = paymentTransaction.Amount.Currency.ToString(),
                    Value = paymentTransaction.Amount.Value
                }
            };
        }
    }
}
