using Moq;
using PaymentGatewayDemo.Infrastructure.Repositories.Entities;
using PaymentGatewayDemo.Infrastructure.Repositories.Transformers;
using PaymentGatewayDemo.Infrastructure.Repositories;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using PaymentGatewayDemo.Domain.Models.Merchant;

namespace PaymentGatewayDemo.Tests.UnitTests.Infrastructure.Repositories
{
    [Trait("Category", "UnitTests")]
    public class MerchantRepositoryTests
    {
        private readonly Mock<IRepository> baseRepositoryMock;
        private readonly Mock<IMerchantDetailsTransformer> merchantTransformerMock;

        private readonly IMerchantRepository repositoryUnderTest;

        public MerchantRepositoryTests()
        {
            this.baseRepositoryMock = new Mock<IRepository>();
            this.merchantTransformerMock = new Mock<IMerchantDetailsTransformer>();

            this.repositoryUnderTest = new MerchantRepository(
                this.baseRepositoryMock.Object,
                this.merchantTransformerMock.Object);
        }

        [Fact]
        public async Task GetMerchantDetails_WithExistingMerchant_ShouldReturnMerchant()
        {
            const string merchantId = "merchant-id";

            var entity = new MerchantDetailsEntity();

            this.baseRepositoryMock
                .Setup(m => m.Query<MerchantDetailsEntity>(merchantId, MerchantDetailsEntity.GetRowKey()))
                .ReturnsAsync(entity);

            var expectedResponse = new MerchantDetails();

            this.merchantTransformerMock
                .Setup(m => m.CreateModel(entity))
                .Returns(expectedResponse);

            var actualResponse = await this.repositoryUnderTest.GetMerchantDetails(merchantId);

            actualResponse.Should().Be(expectedResponse);
        }

        [Fact]
        public async Task GetMerchantDetails_WithMissingMerchant_ShouldReturnNull()
        {
            const string merchantId = "merchant-id";

            this.baseRepositoryMock
                .Setup(m => m.Query<MerchantDetailsEntity>(merchantId, MerchantDetailsEntity.GetRowKey()))
                .ReturnsAsync((MerchantDetailsEntity?)null);

            var actualResponse = await this.repositoryUnderTest.GetMerchantDetails(merchantId);

            actualResponse.Should().BeNull();
        }
    }
}
