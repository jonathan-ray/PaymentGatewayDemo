using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using PaymentGatewayDemo.Application.Services;
using PaymentGatewayDemo.Domain.Models.Merchant;
using PaymentGatewayDemo.Infrastructure.Exceptions;
using PaymentGatewayDemo.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGatewayDemo.Tests.UnitTests.Application.Services
{
    [Trait("Category", "UnitTests")]
    public class UserAuthorizationServiceTests
    {
        private readonly Mock<IMerchantRepository> repositoryMock;

        private readonly IUserAuthorizationService serviceUnderTest;

        public UserAuthorizationServiceTests()
        {
            this.repositoryMock = new Mock<IMerchantRepository>();

            this.serviceUnderTest = new UserAuthorizationService(this.repositoryMock.Object);
        }

        [Fact]
        public async Task VerifyAuthorization_WithUnknownMerchant_ShouldThrowException()
        {
            this.repositoryMock
                .Setup(m => m.GetMerchantDetails(It.IsAny<string>()))
                .ReturnsAsync((MerchantDetails?)null);

            await this.serviceUnderTest
                .Awaiting(s => s.VerifyAuthorization(Mock.Of<HttpRequest>(), "merchant-id"))
                .Should().ThrowAsync<MerchantNotFoundException>();
        }

        [Fact]
        public async Task VerifyAuthorization_WithMissingApiKey_ShouldThrowException()
        {
            var merchant = new MerchantDetails();

            this.repositoryMock
                .Setup(m => m.GetMerchantDetails(It.IsAny<string>()))
                .ReturnsAsync(merchant);

            var request = new Mock<HttpRequest>();
            request
                .Setup(r => r.Headers)
                .Returns(new HeaderDictionary());

            var exceptionAssertion = await this.serviceUnderTest
                .Awaiting(s => s.VerifyAuthorization(request.Object, "merchant-id"))
                .Should().ThrowAsync<AuthorizationFailureException>();

            exceptionAssertion.And.Message.Should().Contain("missing");
        }

        [Fact]
        public async Task VerifyAuthorization_WithIncorrectApiKey_ShouldThrowException()
        {
            var merchant = new MerchantDetails
            {
                ApiKey = "correct"
            };

            this.repositoryMock
                .Setup(m => m.GetMerchantDetails(It.IsAny<string>()))
                .ReturnsAsync(merchant);

            var request = new Mock<HttpRequest>();
            request
                .Setup(r => r.Headers)
                .Returns(new HeaderDictionary(
                    new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
                    {
                        { "PaymentGateway-ApiKey", "incorrect" }
                    }));

            var exceptionAssertion = await this.serviceUnderTest
                .Awaiting(s => s.VerifyAuthorization(request.Object, "merchant-id"))
                .Should().ThrowAsync<AuthorizationFailureException>();

            exceptionAssertion.And.Message.Should().Contain("not valid");
        }

        [Fact]
        public async Task VerifyAuthorization_WithValidAuthorization_ShouldNotThrow()
        {
            var merchant = new MerchantDetails
            {
                MerchantId = "merchant-id",
                ApiKey = "correct"
            };

            this.repositoryMock
                .Setup(m => m.GetMerchantDetails(merchant.MerchantId))
                .ReturnsAsync(merchant);

            var request = new Mock<HttpRequest>();
            request
                .Setup(r => r.Headers)
                .Returns(new HeaderDictionary(
                    new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
                    {
                        { "PaymentGateway-ApiKey", merchant.ApiKey }
                    }));

            await this.serviceUnderTest
                .Awaiting(s => s.VerifyAuthorization(request.Object, merchant.MerchantId))
                .Should().NotThrowAsync();
        }
    }
}
