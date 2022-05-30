using FluentAssertions;
using Moq;
using PaymentGatewayDemo.Domain.Models.PaymentCard;
using PaymentGatewayDemo.Domain.Models.Payments;
using PaymentGatewayDemo.Infrastructure.Repositories.Entities;
using PaymentGatewayDemo.Infrastructure.Repositories.Transformers;
using PaymentGatewayDemo.Infrastructure.Security;
using System;
using Xunit;

namespace PaymentGatewayDemo.Tests.UnitTests.Infrastructure.Repositories.Transformers
{
    [Trait("Category", "UnitTests")]
    public class PaymentTransactionTransformerTests
    {
        private readonly Mock<IDataEncrypter> encrypterMock;

        private readonly IPaymentTransactionTransformer transformerUnderTest;

        public PaymentTransactionTransformerTests()
        {
            this.encrypterMock = new Mock<IDataEncrypter>();

            this.transformerUnderTest = new PaymentTransactionTransformer(this.encrypterMock.Object);
        }

        [Fact]
        public void CreateDatedEntity_WithCorrectModel_ShouldReturnEquivalentEntity()
        {
            const string merchantId = "my-merchant-id";

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

            var entity = this.transformerUnderTest.CreateDatedEntity(merchantId, model);

            entity.Should().NotBeNull();
            entity.MerchantId.Should().Be(merchantId);
            entity.RequestId.Should().Be(model.RequestDetails.Id);
            entity.RequestedOn.Should().Be(model.RequestDetails.RequestedOn);
        }

        [Fact]
        public void CreateDetailsEntity_WithCorrectModel_ShouldReturnEquivalentEntity()
        {
            const string merchantId = "my-merchant-id";

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

            var entity = this.transformerUnderTest.CreateDetailsEntity(merchantId, model);

            entity.Should().NotBeNull();
            entity.MerchantId.Should().Be(merchantId);
            entity.RequestId.Should().Be(model.RequestDetails.Id);
            entity.RequestedOn.Should().Be(model.RequestDetails.RequestedOn);
            entity.CardNumber.Should().Be(model.CardDetails.Number.Value);
            entity.CardExpiryMonth.Should().Be(model.CardDetails.ExpiryDate.Month);
            entity.CardExpiryYear.Should().Be(model.CardDetails.ExpiryDate.Year);
            entity.SecurityValue.Should().Be(model.CardDetails.SecurityDetails.Value);
            entity.Currency.Should().Be((int)model.Amount.Currency);
            entity.Amount.Should().Be(model.Amount.Value);
            entity.ReceiptId.Should().Be(model.ReceiptDetails.ReceiptId);
            entity.ProcessedOn.Should().Be(model.ReceiptDetails.ProcessedOn);
            entity.PaymentStatus.Should().Be((int)model.Status);
        }

        [Fact]
        public void CreateDetailsEntity_WithCorrectModelWithoutReceiptDetails_ShouldReturnEquivalentEntity()
        {
            const string merchantId = "my-merchant-id";

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
                ReceiptDetails = null,
                Status = PaymentStatus.PersistentServerFailure
            };

            var entity = this.transformerUnderTest.CreateDetailsEntity(merchantId, model);

            entity.Should().NotBeNull();
            entity.MerchantId.Should().Be(merchantId);
            entity.RequestId.Should().Be(model.RequestDetails.Id);
            entity.RequestedOn.Should().Be(model.RequestDetails.RequestedOn);
            entity.CardNumber.Should().Be(model.CardDetails.Number.Value);
            entity.CardExpiryMonth.Should().Be(model.CardDetails.ExpiryDate.Month);
            entity.CardExpiryYear.Should().Be(model.CardDetails.ExpiryDate.Year);
            entity.SecurityValue.Should().Be(model.CardDetails.SecurityDetails.Value);
            entity.Currency.Should().Be((int)model.Amount.Currency);
            entity.Amount.Should().Be(model.Amount.Value);
            entity.ReceiptId.Should().Be(null);
            entity.ProcessedOn.Should().Be(null);
            entity.PaymentStatus.Should().Be((int)model.Status);
        }

        [Fact]
        public void CreateModel_WithCorrectEntity_ShouldReturnEquivalentModel()
        {
            var entity = new PaymentTransactionEntity
            {
                MerchantId = "merchant-id",
                RequestedOn = DateTime.UtcNow,
                RequestId = "request-id",
                CardNumber = "1234 4321 2143 3142",
                CardExpiryMonth = 3,
                CardExpiryYear = 25,
                SecurityValue = "914",
                Currency = (int)CurrencyId.Gbp,
                Amount = 500,
                ReceiptId = "receipt-id",
                ProcessedOn = DateTime.UtcNow.AddSeconds(30),
                PaymentStatus = (int)PaymentStatus.Success
            };

            var model = this.transformerUnderTest.CreateModel(entity);

            model.Should().NotBeNull();
            model.RequestDetails.Id.Should().Be(entity.RequestId);
            model.RequestDetails.RequestedOn.Should().Be(entity.RequestedOn);
            model.CardDetails.Number.Value.Should().Be(entity.CardNumber);
            model.CardDetails.ExpiryDate.Month.Should().Be(entity.CardExpiryMonth);
            model.CardDetails.ExpiryDate.Year.Should().Be(entity.CardExpiryYear);
            model.CardDetails.SecurityDetails.Value.Should().Be(entity.SecurityValue);
            model.Amount.Currency.Should().Be((CurrencyId)entity.Currency);
            model.Amount.Value.Should().Be(entity.Amount);
            model.ReceiptDetails.Should().NotBeNull();
            model.ReceiptDetails.ReceiptId.Should().Be(entity.ReceiptId);
            model.ReceiptDetails.ProcessedOn.Should().Be(entity.ProcessedOn);
            model.Status.Should().Be((PaymentStatus)entity.PaymentStatus);
        }

        [Fact]
        public void CreateModel_WithCorrectEntityWithoutReceiptDetails_ShouldReturnEquivalentModel()
        {
            var entity = new PaymentTransactionEntity
            {
                MerchantId = "merchant-id",
                RequestedOn = DateTime.UtcNow,
                RequestId = "request-id",
                CardNumber = "1234 4321 2143 3142",
                CardExpiryMonth = 3,
                CardExpiryYear = 25,
                SecurityValue = "914",
                Currency = (int)CurrencyId.Gbp,
                Amount = 500,
                ReceiptId = null,
                ProcessedOn = null,
                PaymentStatus = (int)PaymentStatus.Success
            };

            var model = this.transformerUnderTest.CreateModel(entity);

            model.Should().NotBeNull();
            model.RequestDetails.Id.Should().Be(entity.RequestId);
            model.RequestDetails.RequestedOn.Should().Be(entity.RequestedOn);
            model.CardDetails.Number.Value.Should().Be(entity.CardNumber);
            model.CardDetails.ExpiryDate.Month.Should().Be(entity.CardExpiryMonth);
            model.CardDetails.ExpiryDate.Year.Should().Be(entity.CardExpiryYear);
            model.CardDetails.SecurityDetails.Value.Should().Be(entity.SecurityValue);
            model.Amount.Currency.Should().Be((CurrencyId)entity.Currency);
            model.Amount.Value.Should().Be(entity.Amount);
            model.ReceiptDetails.Should().BeNull();
            model.Status.Should().Be((PaymentStatus)entity.PaymentStatus);
        }
    }
}
