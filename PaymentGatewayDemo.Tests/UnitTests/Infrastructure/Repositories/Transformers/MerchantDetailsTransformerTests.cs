using FluentAssertions;
using Moq;
using PaymentGatewayDemo.Domain.Models.Merchant;
using PaymentGatewayDemo.Infrastructure.Repositories.Entities;
using PaymentGatewayDemo.Infrastructure.Repositories.Transformers;
using PaymentGatewayDemo.Infrastructure.Security;
using System;
using Xunit;

namespace PaymentGatewayDemo.Tests.UnitTests.Infrastructure.Repositories.Transformers
{
    [Trait("Category", "UnitTests")]
    public class MerchantDetailsTransformerTests
    {
        private readonly Mock<IDataEncrypter> encrypterMock;

        private readonly IMerchantDetailsTransformer transformerUnderTest;

        public MerchantDetailsTransformerTests()
        {
            this.encrypterMock = new Mock<IDataEncrypter>();

            this.transformerUnderTest = new MerchantDetailsTransformer(this.encrypterMock.Object);
        }

        [Fact]
        public void CreateEntity_WithCorrectModel_ShouldReturnEquivalentEntity()
        {
            const string apiKey = "123-234-345";
            const string encryptedApiKey = "000-111-222";

            var model = new MerchantDetails
            {
                MerchantId = "abc",
                ApiKey = apiKey,
                AcquiringBank = Guid.NewGuid(),
                MerchantBankId = "abc-123"
            };

            this.encrypterMock
                .Setup(m => m.Encrypt(apiKey))
                .Returns(encryptedApiKey);

            var entity = this.transformerUnderTest.CreateEntity(model);

            entity.Should().NotBeNull();
            entity.MerchantId.Should().Be(model.MerchantId);
            entity.ApiKey.Should().Be(encryptedApiKey);
            entity.AcquiringBank.Should().Be(model.AcquiringBank);
            entity.MerchantBankId.Should().Be(model.MerchantBankId);
        }

        [Fact]
        public void CreateModel_WithCorrectEntity_ShouldReturnEquivalentModel()
        {
            const string encryptedApiKey = "000-111-222";
            const string apiKey = "123-234-345";

            var entity = new MerchantDetailsEntity
            {
                MerchantId = "cde",
                ApiKey = encryptedApiKey,
                AcquiringBank = Guid.NewGuid(),
                MerchantBankId = "cde-123"
            };

            this.encrypterMock
                .Setup(m => m.Decrypt(encryptedApiKey))
                .Returns(apiKey);

            var model = this.transformerUnderTest.CreateModel(entity);

            model.Should().NotBeNull();
            model.MerchantId.Should().Be(entity.MerchantId);
            model.ApiKey.Should().Be(apiKey);
            model.AcquiringBank.Should().Be(entity.AcquiringBank);
            model.MerchantBankId.Should().Be(entity.MerchantBankId);
        }
    }
}
