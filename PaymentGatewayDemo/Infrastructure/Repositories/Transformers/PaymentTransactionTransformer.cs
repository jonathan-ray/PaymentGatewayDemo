using PaymentGatewayDemo.Domain.Models.PaymentCard;
using PaymentGatewayDemo.Domain.Models.Payments;
using PaymentGatewayDemo.Infrastructure.Repositories.Entities;
using PaymentGatewayDemo.Infrastructure.Security;

namespace PaymentGatewayDemo.Infrastructure.Repositories.Transformers;

public class PaymentTransactionTransformer : IPaymentTransactionTransformer
{
    // TODO: Use data encryption.
    private readonly IDataEncrypter dataEncrypter;

    public PaymentTransactionTransformer(IDataEncrypter dataEncrypter)
    {
        this.dataEncrypter = dataEncrypter ?? throw new ArgumentNullException(nameof(dataEncrypter));
    }

    public DatedPaymentEntity CreateDatedEntity(string merchantId, PaymentTransaction model)
    {
        return new DatedPaymentEntity
        {
            MerchantId = merchantId,
            RequestedOn = model.RequestDetails.RequestedOn,
            RequestId = model.RequestDetails.Id
        };
    }

    public PaymentTransactionEntity CreateDetailsEntity(string merchantId, PaymentTransaction model)
    {
        return new PaymentTransactionEntity
        {
            MerchantId = merchantId,
            RequestedOn = model.RequestDetails.RequestedOn,
            RequestId = model.RequestDetails.Id,
            CardNumber = model.CardDetails.Number.Value,
            CardExpiryMonth = model.CardDetails.ExpiryDate.Month,
            CardExpiryYear = model.CardDetails.ExpiryDate.Year,
            SecurityValue = model.CardDetails.SecurityDetails.Value,
            Currency = (int)model.Amount.Currency,
            Amount = model.Amount.Value,
            ReceiptId = model.ReceiptDetails?.ReceiptId,
            ProcessedOn = model.ReceiptDetails?.ProcessedOn,
            PaymentStatus = (int)model.Status
        };
    }

    public PaymentTransaction CreateModel(PaymentTransactionEntity entity)
    {
        return new PaymentTransaction
        {
            RequestDetails = new PaymentRequestDetails()
            {
                Id = entity.RequestId,
                RequestedOn = entity.RequestedOn
            },
            CardDetails = new CardDetails
            {
                Number = new CardNumber
                {
                    Value = entity.CardNumber
                },
                ExpiryDate = new MonthlyDate
                {
                    Month = entity.CardExpiryMonth,
                    Year = entity.CardExpiryYear
                },
                SecurityDetails = new SecurityDetails(entity.SecurityValue)
            },
            Amount = new CurrencyAmount
            {
                Currency = (CurrencyId)entity.Currency,
                Value = entity.Amount
            },
            ReceiptDetails = entity.ReceiptId is not null && entity.ProcessedOn.HasValue
                ? new PaymentReceiptDetails(entity.ReceiptId, entity.ProcessedOn.Value)
                : null,
            Status = (PaymentStatus)entity.PaymentStatus
        };
    }
}
