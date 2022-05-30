using FluentAssertions;
using Moq;
using PaymentGatewayDemo.Application.Facades;
using PaymentGatewayDemo.Domain.Models.AcquiringBank;
using PaymentGatewayDemo.Domain.Models.Payments;
using PaymentGatewayDemo.Domain.Services;
using PaymentGatewayDemo.Domain.Transformers;
using PaymentGatewayDemo.Infrastructure.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGatewayDemo.Tests.UnitTests.Application.Facades
{
    [Trait("Category", "UnitTests")]
    public class AcquiringBankServiceFacadeTests
    {
        private const string DefaultMerchantId = "my-merchant-id";

        private readonly Mock<IAcquiringBankPaymentProcessingService<ProcessPaymentRequestModel, ProcessPaymentResponseModel>> bankServiceMock;
        private readonly Mock<IAcquiringBankModelTransformer<ProcessPaymentRequestModel, ProcessPaymentResponseModel>> modelTransformerMock;

        private readonly IAcquiringBankServiceFacade facadeUnderTest;

        public AcquiringBankServiceFacadeTests()
        {
            this.bankServiceMock = new Mock<IAcquiringBankPaymentProcessingService<ProcessPaymentRequestModel, ProcessPaymentResponseModel>>();
            this.modelTransformerMock = new Mock<IAcquiringBankModelTransformer<ProcessPaymentRequestModel, ProcessPaymentResponseModel>>();

            this.facadeUnderTest = new AcquiringBankServiceFacade(
                DefaultMerchantId,
                this.bankServiceMock.Object,
                this.modelTransformerMock.Object);
        }

        [Fact]
        public async Task ProcessTransaction_WithCompleteTransaction_ShouldReturnResult()
        {
            var transactionRequest = new PaymentTransaction();

            var modelRequest = new ProcessPaymentRequestModel();

            this.modelTransformerMock
                .Setup(m => m.CreateProcessRequestFromTransaction(DefaultMerchantId, transactionRequest))
                .Returns(modelRequest);

            var modelResponse = new ProcessPaymentResponseModel();

            this.bankServiceMock
                .Setup(m => m.ProcessPayment(modelRequest))
                .ReturnsAsync(modelResponse);

            var expectedResult = new PaymentTransactionResult(PaymentStatus.Success);

            this.modelTransformerMock
                .Setup(m => m.CreateTransactionResultFromProcessResponse(modelResponse))
                .Returns(expectedResult);

            var actualResult = await this.facadeUnderTest.ProcessTransaction(transactionRequest);

            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task ProcessTransaction_WithProcessingFailure_ShouldThrowException()
        {
            var transactionRequest = new PaymentTransaction();

            var modelRequest = new ProcessPaymentRequestModel();

            this.modelTransformerMock
                .Setup(m => m.CreateProcessRequestFromTransaction(DefaultMerchantId, transactionRequest))
                .Returns(modelRequest);

            var internalException = new ArithmeticException();

            this.bankServiceMock
                .Setup(m => m.ProcessPayment(modelRequest))
                .ThrowsAsync(internalException);

            var exceptionAssertion = await this.facadeUnderTest
                .Awaiting(f => f.ProcessTransaction(transactionRequest))
                .Should().ThrowAsync<AcquiringBankServiceException>();

            exceptionAssertion.And.InnerException.Should().Be(internalException);
        }
    }
}
