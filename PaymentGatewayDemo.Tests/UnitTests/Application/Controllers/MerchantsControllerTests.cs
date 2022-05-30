using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using PaymentGatewayDemo.Application.Controllers;
using PaymentGatewayDemo.Application.Services;
using PaymentGatewayDemo.Domain.Models.Payments;
using PaymentGatewayDemo.Domain.Transformers;
using PaymentGatewayDemo.Infrastructure.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGatewayDemo.Tests
{
    [Trait("Category", "UnitTests")]
    public class MerchantsControllerTests
    {
        private readonly Mock<IUserAuthorizationService> authServiceMock;
        private readonly Mock<IPaymentService> paymentServiceMock;
        private readonly Mock<IPaymentModelTransformer> modelTransformerMock;

        private readonly MerchantsController controllerUnderTest;

        public MerchantsControllerTests()
        {
            this.authServiceMock = new Mock<IUserAuthorizationService>();
            this.paymentServiceMock = new Mock<IPaymentService>();
            this.modelTransformerMock = new Mock<IPaymentModelTransformer>();

            this.controllerUnderTest = new MerchantsController(
                this.authServiceMock.Object,
                this.paymentServiceMock.Object, 
                this.modelTransformerMock.Object);
        }

        [Fact]
        public void Construction_WithNullAuthorizationService_ShouldThrowException()
        {
            Func<MerchantsController> construction = () => new MerchantsController(
                authorizationService: null!,
                Mock.Of<IPaymentService>(),
                Mock.Of<IPaymentModelTransformer>());

            construction
                .Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("authorizationService");
        }

        [Fact]
        public void Construction_WithNullPaymentService_ShouldThrowException()
        {
            Func<MerchantsController> construction = () => new MerchantsController(
                Mock.Of<IUserAuthorizationService>(),
                paymentService: null!,
                Mock.Of<IPaymentModelTransformer>());

            construction
                .Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("paymentService");
        }

        [Fact]
        public void Construction_WithNullPaymentModelTransformer_ShouldThrowException()
        {
            Func<MerchantsController> construction = () => new MerchantsController(
                Mock.Of<IUserAuthorizationService>(),
                Mock.Of<IPaymentService>(),
                paymentModelTransformer: null!);

            construction
                .Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("paymentModelTransformer");
        }

        [Fact]
        public async Task GetPaymentTransaction_WithFailedAuthorization_ShouldRethrowException()
        {
            const string errorDescription = "a specific error description";

            this.authServiceMock
                .Setup(m => m.VerifyAuthorization(It.IsAny<HttpRequest>(), It.IsAny<string>()))
                .ThrowsAsync(new AuthorizationFailureException(errorDescription));

            var exceptionAssertion = await this.controllerUnderTest
                .Awaiting(c => c.GetPaymentTransaction("merchant", "request-ID"))
                .Should().ThrowAsync<AuthorizationFailureException>();
            exceptionAssertion.And.Message.Should().Contain(errorDescription);
        }

        [Fact]
        public async Task GetPaymentTransaction_WithPaymentNotFound_ShouldThrowException()
        {
            this.paymentServiceMock
                .Setup(m => m.GetPaymentTransaction(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((PaymentTransaction?)null);

            await this.controllerUnderTest
                .Awaiting(c => c.GetPaymentTransaction("merchant", "request-ID"))
                .Should().ThrowAsync<PaymentTransactionNotFoundException>();
        }

        [Fact]
        public async Task GetPaymentTransaction_WithPaymentFound_ShouldReturnPaymentDetails()
        {
            const string merchantId = "merchant";
            const string requestId = "request-id";

            var transaction = new PaymentTransaction();

            this.paymentServiceMock
                .Setup(m => m.GetPaymentTransaction(merchantId, requestId))
                .ReturnsAsync(transaction);

            var expectedResponse = new PaymentTransactionModel<PaymentReceiptModel>();

            this.modelTransformerMock
                .Setup(m => m.CreateTransactionResponseFromTransaction(transaction))
                .Returns(expectedResponse);

            var actualResponse = await this.controllerUnderTest.GetPaymentTransaction(merchantId, requestId);
            actualResponse.Should().Be(expectedResponse);
        }

        [Fact]
        public async Task ProcessPayment_WithFailedAuthorization_ShouldThrowException()
        {
            const string merchantId = "merchant";

            var request = new PaymentTransactionModel<PaymentRequestModel>();
            var transactionRequest = new PaymentTransaction
            {
                RequestDetails = new PaymentRequestDetails()
            };

            this.modelTransformerMock
                .Setup(m => m.CreateTransactionFromRequest(request))
                .Returns(transactionRequest);

            var transactionResult = new PaymentTransactionResult(PaymentStatus.PaymentDenied);

            this.paymentServiceMock
                .Setup(m => m.ProcessPaymentTransaction(merchantId, transactionRequest))
                .ReturnsAsync(transactionResult);

            var expectedResponse = new PaymentReceiptModel();

            this.modelTransformerMock
                .Setup(m => m.CreateReceiptFromResult(transactionRequest.RequestDetails, transactionResult))
                .Returns(expectedResponse);

            var actualResponse = await this.controllerUnderTest.ProcessPayment(merchantId, request);
            actualResponse.Should().Be(expectedResponse);
        }

        [Fact]
        public async Task ProcessPayment_WithCompletePayment_ShouldReturnPaymentReceipt()
        {
            const string errorDescription = "a specific error description";

            this.authServiceMock
                .Setup(m => m.VerifyAuthorization(It.IsAny<HttpRequest>(), It.IsAny<string>()))
                .ThrowsAsync(new AuthorizationFailureException(errorDescription));

            var exceptionAssertion = await this.controllerUnderTest
                .Awaiting(c => c.ProcessPayment("merchant", new PaymentTransactionModel<PaymentRequestModel>()))
                .Should().ThrowAsync<AuthorizationFailureException>();
            exceptionAssertion.And.Message.Should().Contain(errorDescription);
        }
    }
}