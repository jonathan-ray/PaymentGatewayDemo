using FluentAssertions;
using PaymentGatewayDemo.Domain.Models.PaymentCard;
using PaymentGatewayDemo.Domain.Models.Payments;
using PaymentGatewayDemo.Domain.Transformers;
using System;
using Xunit;

namespace PaymentGatewayDemo.Tests.UnitTests.Domain.Transformers
{
    [Trait("Category", "UnitTests")]
    public class PaymentModelTransformerTests
    {
        private readonly IPaymentModelTransformer transformerUnderTest;

        public PaymentModelTransformerTests()
        {
            this.transformerUnderTest = new PaymentModelTransformer();
        }

        [Fact]
        public void CreateReceiptFromResult_WithCorrectResult_ShouldReturnEquivalentReceipt()
        {
            var request = new PaymentRequestDetails
            {
                Id = "111222333",
                RequestedOn = DateTime.UtcNow.AddDays(-1)
            };

            var result = new PaymentTransactionResult(PaymentStatus.Success, new PaymentReceiptDetails("aaabbbcccc", DateTime.UtcNow.AddHours(-23)));

            var receipt = this.transformerUnderTest.CreateReceiptFromResult(request, result);

            receipt.Should().NotBeNull();
            receipt.Status.Should().Be(PaymentStatus.Success.ToString());
            receipt.Request.Id.Should().Be(request.Id);
            receipt.Request.RequestedOn.Should().Be(request.RequestedOn);
            receipt.BankReceiptId.Should().Be(result.Receipt.ReceiptId);
            receipt.ProcessedOn.Should().Be(result.Receipt.ProcessedOn);
        }

        [Fact]
        public void CreateReceiptFromResult_WithCorrectResultWithoutReceiptDetails_ShouldReturnEquivalentReceipt()
        {
            var request = new PaymentRequestDetails
            {
                Id = "11122233344444",
                RequestedOn = DateTime.UtcNow.AddDays(-2)
            };

            var result = new PaymentTransactionResult(PaymentStatus.PersistentServerFailure);

            var receipt = this.transformerUnderTest.CreateReceiptFromResult(request, result);

            receipt.Should().NotBeNull();
            receipt.Status.Should().Be(PaymentStatus.PersistentServerFailure.ToString());
            receipt.Request.Id.Should().Be(request.Id);
            receipt.Request.RequestedOn.Should().Be(request.RequestedOn);
            receipt.BankReceiptId.Should().BeNull();
            receipt.ProcessedOn.Should().BeNull();
        }

        [Fact]
        public void CreateTransactionResponseFromTransaction_WithCorrectTransaction_ShouldReturnEquivalentResponse()
        {
            var model = new PaymentTransaction
            {
                RequestDetails = new PaymentRequestDetails()
                {
                    Id = "request-id",
                    RequestedOn = DateTime.UtcNow
                },
                CardDetails = new CardDetails
                {
                    Number = new CardNumber
                    {
                        Value = "1234 4321 2143 3142"
                    },
                    ExpiryDate = new MonthlyDate
                    {
                        Month = 8,
                        Year = 23
                    },
                    SecurityDetails = new SecurityDetails("007")
                },
                Amount = new CurrencyAmount
                {
                    Currency = CurrencyId.Eur,
                    Value = 200
                },
                ReceiptDetails = new PaymentReceiptDetails("qwerty", DateTime.UtcNow.AddSeconds(20)),
                Status = PaymentStatus.Success
            };

            var response = this.transformerUnderTest.CreateTransactionResponseFromTransaction(model);

            response.Should().NotBeNull();
            response.Details.Should().NotBeNull();
            response.Details.Status.Should().Be(PaymentStatus.Success.ToString());
            response.Details.Request.Id.Should().Be(model.RequestDetails.Id);
            response.Details.Request.RequestedOn.Should().Be(model.RequestDetails.RequestedOn);
            response.Details.BankReceiptId.Should().Be(model.ReceiptDetails.ReceiptId);
            response.Details.ProcessedOn.Should().Be(model.ReceiptDetails.ProcessedOn);
            response.PaymentCard.CardNumber.Should().NotBeNull();
            response.PaymentCard.CardNumber.Should().NotBe(model.CardDetails.Number.Value); // Should be hashed
            response.PaymentCard.ExpiryMonth.Should().Be(model.CardDetails.ExpiryDate.Month);
            response.PaymentCard.ExpiryYear.Should().Be(model.CardDetails.ExpiryDate.Year);
            response.PaymentCard.CVV.Should().BeNull(); // Should be omitted for privacy reasons
            response.Amount.Currency.Should().Be(CurrencyId.Eur.ToString());
            response.Amount.Value.Should().Be(model.Amount.Value);
        }

        [Fact]
        public void CreateTransactionFromRequest_WithCorrectRequest_ShouldReturnEquivalentTransaction()
        {
            var request = new PaymentTransactionModel<PaymentRequestModel>
            {
                Details = new PaymentRequestModel
                {
                    Id = "request-id",
                    RequestedOn = DateTime.UtcNow
                },
                PaymentCard = new PaymentCardModel
                {
                    CardNumber = "5678 8765 5867 7856",
                    ExpiryMonth = 7,
                    ExpiryYear = 27,
                    CVV = "113"
                },
                Amount = new CurrencyAmountModel
                {
                    Currency = "Brl",
                    Value = 32
                }
            };

            var transaction = this.transformerUnderTest.CreateTransactionFromRequest(request);

            transaction.Should().NotBeNull();
            transaction.RequestDetails.Id.Should().Be(request.Details.Id);
            transaction.RequestDetails.RequestedOn.Should().Be(request.Details.RequestedOn);
            transaction.CardDetails.Number.Value.Should().Be(request.PaymentCard.CardNumber);
            transaction.CardDetails.ExpiryDate.Month.Should().Be(request.PaymentCard.ExpiryMonth);
            transaction.CardDetails.ExpiryDate.Year.Should().Be(request.PaymentCard.ExpiryYear);
            transaction.CardDetails.SecurityDetails.Value.Should().Be(request.PaymentCard.CVV);
            transaction.Amount.Currency.Should().Be(CurrencyId.Brl);
            transaction.Amount.Value.Should().Be(request.Amount.Value);
        }
    }
}
