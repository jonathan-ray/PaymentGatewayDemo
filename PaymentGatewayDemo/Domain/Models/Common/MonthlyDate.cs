namespace PaymentGatewayDemo.Domain.Models.PaymentCard;

public class MonthlyDate
{
    /// <summary>
    /// Month of the date as represented by its integer value.
    /// </summary>
    public uint Month { get; init; }

    /// <summary>
    /// Year of the date.
    /// </summary>
    public uint Year { get; init; }
}
