using PaymentGatewayDemo.Domain.Models.PaymentCard;

namespace PaymentGatewayDemo.Domain.Models.Payments;

public class PaymentTransactionModel<TPaymentModel>
{
    /// <summary>
    /// Details associated with the given transaction.
    /// </summary>
    public TPaymentModel Details { get; init; }

    /// <summary>
    /// Payment card for the payment.
    /// </summary>
    public PaymentCardModel PaymentCard { get; init; }

    /// <summary>
    /// The amount to be paid.
    /// </summary>
    public CurrencyAmountModel Amount { get; init; }
}
