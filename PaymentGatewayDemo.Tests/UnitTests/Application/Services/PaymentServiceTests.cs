using FluentAssertions;
using Moq;
using PaymentGatewayDemo.Application.Facades;
using PaymentGatewayDemo.Application.Factories;
using PaymentGatewayDemo.Application.Services;
using PaymentGatewayDemo.Domain.Models.Payments;
using PaymentGatewayDemo.Infrastructure.Exceptions;
using PaymentGatewayDemo.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGatewayDemo.Tests.UnitTests.Application.Services
{
    [Trait("Category", "UnitTests")]
    public class PaymentServiceTests
    {
        private readonly Mock<IAcquiringBankServiceFacadeFactory> facadeFactoryMock;
        private readonly Mock<IAcquiringBankServiceFacade> facadeMock;
        private readonly Mock<IPaymentRepository> repositoryMock;

        private readonly IPaymentService serviceUnderTest;

        public PaymentServiceTests()
        {
            this.facadeMock = new Mock<IAcquiringBankServiceFacade>();
            this.facadeFactoryMock = new Mock<IAcquiringBankServiceFacadeFactory>();
            this.repositoryMock = new Mock<IPaymentRepository>();

            this.serviceUnderTest = new PaymentService(
                this.facadeFactoryMock.Object,
                this.repositoryMock.Object);
        }

        [Fact]
        public async Task GetPaymentTransaction_WithKnownTransaction_ShouldReturnTransaction()
        {
            const string merchantId = "merchant-id";
            const string paymentId = "payment-id";

            var expectedTransaction = new PaymentTransaction();

            this.repositoryMock
                .Setup(r => r.GetPaymentTransaction(merchantId, paymentId))
                .ReturnsAsync(expectedTransaction);

            var actualTransaction = await this.serviceUnderTest.GetPaymentTransaction(merchantId, paymentId);

            actualTransaction.Should().Be(expectedTransaction);
        }

        [Fact]
        public async Task GetPaymentTransaction_WithUnknownTransaction_ShouldReturnNull()
        {
            const string merchantId = "merchant-id";
            const string paymentId = "payment-id";

            this.repositoryMock
                .Setup(r => r.GetPaymentTransaction(merchantId, paymentId))
                .ReturnsAsync((PaymentTransaction?)null);

            var actualTransaction = await this.serviceUnderTest.GetPaymentTransaction(merchantId, paymentId);

            actualTransaction.Should().BeNull();
        }

        [Fact]
        public async Task ProcessPaymentTransaction_WithAlreadyProcessedTransaction_ShouldReturnTransaction()
        {
            const string merchantId = "merchant-id";
            var transactionRequest = new PaymentTransaction
            {
                RequestDetails = new PaymentRequestDetails
                {
                    Id = "a1b2",
                    RequestedOn = new DateTime(2022, 2, 2, 2, 2, 2, DateTimeKind.Utc)
                }
            };

            var expectedResult = new PaymentTransactionResult(PaymentStatus.Success, new PaymentReceiptDetails("abc", DateTime.UtcNow));

            var idempotentTransaction = new PaymentTransaction
            {
                RequestDetails = new PaymentRequestDetails
                {
                    Id = transactionRequest.RequestDetails.Id,
                    RequestedOn = transactionRequest.RequestDetails.RequestedOn,
                },
                Status = expectedResult.Status,
                ReceiptDetails = expectedResult.Receipt
            };

            this.repositoryMock
                .Setup(m => m.GetPaymentTransaction(merchantId, transactionRequest.RequestDetails.Id))
                .ReturnsAsync(idempotentTransaction);

            var actualResult = await this.serviceUnderTest.ProcessPaymentTransaction(merchantId, transactionRequest);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ProcessPaymentTransaction_WithReusedRequestId_ShouldThrowException()
        {
            const string merchantId = "merchant-id";
            var transactionRequest = new PaymentTransaction
            {
                RequestDetails = new PaymentRequestDetails
                {
                    Id = "a1b2",
                    RequestedOn = new DateTime(2022, 2, 2, 2, 2, 2, DateTimeKind.Utc)
                }
            };

            var expectedReceipt = new PaymentReceiptDetails("abc", DateTime.UtcNow);

            var idempotentTransaction = new PaymentTransaction
            {
                RequestDetails = new PaymentRequestDetails
                {
                    Id = transactionRequest.RequestDetails.Id,
                    RequestedOn = new DateTime(2022, 3, 3, 3, 3, 3, DateTimeKind.Utc)
                },
                ReceiptDetails = expectedReceipt
            };

            this.repositoryMock
                .Setup(m => m.GetPaymentTransaction(merchantId, transactionRequest.RequestDetails.Id))
                .ReturnsAsync(idempotentTransaction);

            await this.serviceUnderTest
                .Awaiting(s => s.ProcessPaymentTransaction(merchantId, transactionRequest))
                .Should().ThrowAsync<IdempotentPaymentTransactionException>();
        }

        [Fact]
        public async Task ProcessPaymentTransaction_WithNewTransaction_ShouldProcessTransaction()
        {
            const string merchantId = "merchant-id";
            var transactionRequest = new PaymentTransaction
            {
                RequestDetails = new PaymentRequestDetails
                {
                    Id = "a1b2",
                    RequestedOn = new DateTime(2022, 2, 2, 2, 2, 2, DateTimeKind.Utc)
                }
            };

            this.repositoryMock
                .Setup(m => m.GetPaymentTransaction(merchantId, transactionRequest.RequestDetails.Id))
                .ReturnsAsync((PaymentTransaction?)null);

            this.facadeFactoryMock
                .Setup(f => f.GetAcquiringBankForMerchant(It.IsAny<string>()))
                .ReturnsAsync(this.facadeMock.Object);

            var expectedResult = new PaymentTransactionResult(PaymentStatus.Success, new PaymentReceiptDetails("a1b2", DateTime.UtcNow));

            this.facadeMock
                .Setup(m => m.ProcessTransaction(transactionRequest))
                .ReturnsAsync(expectedResult);


            var actualReceipt = await this.serviceUnderTest.ProcessPaymentTransaction(merchantId, transactionRequest);

            actualReceipt.Should().Be(expectedResult);

            this.repositoryMock.Verify(m => m.UpsertPaymentTransaction(
                merchantId, 
                It.Is<PaymentTransaction>(t => 
                    t.RequestDetails == transactionRequest.RequestDetails &&
                    t.ReceiptDetails.ReceiptId == expectedResult.Receipt.ReceiptId &&
                    t.ReceiptDetails.ProcessedOn == expectedResult.Receipt.ProcessedOn &&
                    t.Status == expectedResult.Status)), 
                Times.Once);
        }
    }
}
