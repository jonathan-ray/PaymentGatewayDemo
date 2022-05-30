using FluentAssertions;
using Moq;
using PaymentGatewayDemo.Domain.Models.Payments;
using PaymentGatewayDemo.Infrastructure.Repositories;
using PaymentGatewayDemo.Infrastructure.Repositories.Entities;
using PaymentGatewayDemo.Infrastructure.Repositories.Transformers;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGatewayDemo.Tests.UnitTests.Infrastructure.Repositories
{
    [Trait("Category", "UnitTests")]
    public class PaymentRepositoryTests
    {
        private readonly Mock<IRepository> baseRepositoryMock;
        private readonly Mock<IPaymentTransactionTransformer> transactionTransformerMock;

        private readonly IPaymentRepository repositoryUnderTest;

        public PaymentRepositoryTests()
        {
            this.baseRepositoryMock = new Mock<IRepository>();
            this.transactionTransformerMock = new Mock<IPaymentTransactionTransformer>();

            this.repositoryUnderTest = new PaymentRepository(
                this.baseRepositoryMock.Object,
                this.transactionTransformerMock.Object);
        }

        [Fact]
        public async Task GetPaymentTransaction_WithExistingTransaction_ShouldReturnTransaction()
        {
            const string merchantId = "merchant-id";
            const string paymentId = "payment-request-id";

            var entity = new PaymentTransactionEntity();

            this.baseRepositoryMock
                .Setup(m => m.Query<PaymentTransactionEntity>(merchantId, PaymentTransactionEntity.GetRowKey(paymentId)))
                .ReturnsAsync(entity);

            var expectedResponse = new PaymentTransaction();

            this.transactionTransformerMock
                .Setup(m => m.CreateModel(entity))
                .Returns(expectedResponse);

            var actualResponse = await this.repositoryUnderTest.GetPaymentTransaction(merchantId, paymentId);

            actualResponse.Should().Be(expectedResponse);
        }

        [Fact]
        public async Task GetPaymentTransaction_WithMissingTransaction_ShouldReturnNull()
        {
            const string merchantId = "merchant-id";
            const string paymentId = "payment-request-id";

            this.baseRepositoryMock
                .Setup(m => m.Query<PaymentTransactionEntity>(merchantId, PaymentTransactionEntity.GetRowKey(paymentId)))
                .ReturnsAsync((PaymentTransactionEntity?)null);

            var actualResponse = await this.repositoryUnderTest.GetPaymentTransaction(merchantId, paymentId);

            actualResponse.Should().BeNull();
        }

        [Fact]
        public async Task UpsertPaymentTransaction_WithTransaction_ShouldUpsertSuccessfully()
        {
            const string merchantId = "merchant-id";
            var paymentDetails = new PaymentTransaction();

            var detailsEntity = new PaymentTransactionEntity();
            this.transactionTransformerMock.Setup(m => m.CreateDetailsEntity(merchantId, paymentDetails))
                .Returns(detailsEntity);

            var datedEntity = new DatedPaymentEntity();
            this.transactionTransformerMock.Setup(m => m.CreateDatedEntity(merchantId, paymentDetails))
                .Returns(datedEntity);

            await this.repositoryUnderTest.UpsertPaymentTransaction(merchantId, paymentDetails);

            this.baseRepositoryMock.Verify(m => m.Upsert(detailsEntity), Times.Once);
            this.baseRepositoryMock.Verify(m => m.Upsert(datedEntity), Times.Once);
        }
    }
}
