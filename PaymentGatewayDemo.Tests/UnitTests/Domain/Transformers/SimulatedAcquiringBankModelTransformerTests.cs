using FluentAssertions;
using PaymentGatewayDemo.Domain.Models.AcquiringBank;
using PaymentGatewayDemo.Domain.Models.PaymentCard;
using PaymentGatewayDemo.Domain.Models.Payments;
using PaymentGatewayDemo.Domain.Transformers;
using System;
using Xunit;

namespace PaymentGatewayDemo.Tests.UnitTests.Domain.Transformers
{
    [Trait("Category", "UnitTests")]
    public class SimulatedAcquiringBankModelTransformerTests
    {
        private readonly IAcquiringBankModelTransformer<ProcessPaymentRequestModel, ProcessPaymentResponseModel> transformerUnderTest;

        public SimulatedAcquiringBankModelTransformerTests()
        {
            this.transformerUnderTest = new SimulatedAcquiringBankModelTransformer();
        }

        [Fact]
        public void CreateProcessRequestFromTransaction_WithCorrectTransaction_ShouldReturnEquivalentProcessRequest()
        {
            const string merchantId = "my-merchant-bank-id";

            var transaction = new PaymentTransaction
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
                Status = PaymentStatus.Pending
            };

            var request = this.transformerUnderTest.CreateProcessRequestFromTransaction(merchantId, transaction);

            request.Should().NotBeNull();
            request.MerchantId.Should().Be(merchantId);
            request.RequestId.Should().Be(transaction.RequestDetails.Id);
            request.PaymentCardNumber.Should().Be(transaction.CardDetails.Number.Value);
            request.PaymentCardExpiryMonth.Should().Be(transaction.CardDetails.ExpiryDate.Month);
            request.PaymentCardExpiryYear.Should().Be(transaction.CardDetails.ExpiryDate.Year);
            request.CvvNumber.Should().Be(transaction.CardDetails.SecurityDetails.Value);
            request.Currency.Should().Be(transaction.Amount.Currency.ToString());
            request.Amount.Should().Be(transaction.Amount.Value);
        }

        [Fact]
        public void CreateTransactionResultFromProcessResponse_WithCorrectResponse_ShouldReturnEquivalentResult()
        {
            var response = new ProcessPaymentResponseModel
            {
                ReceiptId = "abc-123",
                ProcessedOn = DateTime.UtcNow,
                Status = ProcessPaymentStatusModel.Success,
            };

            var result = this.transformerUnderTest.CreateTransactionResultFromProcessResponse(response);

            result.Should().NotBeNull();
            result.Receipt.Should().NotBeNull();
            result.Receipt.ReceiptId.Should().Be(response.ReceiptId);
            result.Receipt.ProcessedOn.Should().Be(response.ProcessedOn);
            result.Status.Should().Be(PaymentStatus.Success);
        }
    }
}
