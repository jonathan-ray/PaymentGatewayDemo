using FluentAssertions;
using PaymentGatewayDemo.Infrastructure.Repositories;
using PaymentGatewayDemo.Infrastructure.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGatewayDemo.Tests.UnitTests.Infrastructure.Repositories
{
    [Trait("Category", "UnitTests")]
    public class GenericRepositoryTests
    {
        private const string DefaultPartitionKey = "my-partition-key";
        private const string DefaultRowKey = "my-row-key";
        private static readonly TableEntity DefaultEntity = new()
        {
            PartitionKey = DefaultPartitionKey,
            RowKey = DefaultRowKey,
            Value = 1337
        };

        private readonly IRepository repositoryUnderTest;

        public GenericRepositoryTests()
        {
            var seed = new Dictionary<string, IDictionary<string, ITableEntity>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    DefaultPartitionKey,
                    new Dictionary<string, ITableEntity>(StringComparer.OrdinalIgnoreCase)
                    {
                        {
                            DefaultRowKey,
                            DefaultEntity
                        }
                    }
                }
            };

            this.repositoryUnderTest = new GenericRepository(seed);
        }

        [Fact]
        public async Task Query_WithUnknownPartitionKey_ShouldReturnNull()
        {
            var result = await this.repositoryUnderTest.Query<TableEntity>("unknown", DefaultRowKey);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Query_WithUnknownRowKey_ShouldReturnNull()
        {
            var result = await this.repositoryUnderTest.Query<TableEntity>(DefaultPartitionKey, "unknown");

            result.Should().BeNull();
        }

        [Fact]
        public async Task Query_WithExistingRow_ShouldReturnRow()
        {
            var result = await this.repositoryUnderTest.Query<TableEntity>(DefaultPartitionKey, DefaultRowKey);

            result.Should().Be(DefaultEntity);
        }

        [Fact]
        public async Task Upsert_PartitionDoesNotExist_ShouldInsertSuccessfully()
        {
            var entity = new TableEntity
            {
                PartitionKey = "new-partition",
                RowKey = DefaultRowKey
            };

            var result = await this.repositoryUnderTest.Query<TableEntity>(entity.PartitionKey, entity.RowKey);
            result.Should().BeNull();

            await this.repositoryUnderTest.Upsert(entity);

            result = await this.repositoryUnderTest.Query<TableEntity>(entity.PartitionKey, entity.RowKey);
            result.Should().Be(entity);
        }

        [Fact]
        public async Task Upsert_RowDoesNotExist_ShouldInsertSuccessfully()
        {
            var entity = new TableEntity
            {
                PartitionKey = DefaultPartitionKey,
                RowKey = "new-row"
            };

            var result = await this.repositoryUnderTest.Query<TableEntity>(entity.PartitionKey, entity.RowKey);
            result.Should().BeNull();

            await this.repositoryUnderTest.Upsert(entity);

            result = await this.repositoryUnderTest.Query<TableEntity>(entity.PartitionKey, entity.RowKey);
            result.Should().Be(entity);
        }

        [Fact]
        public async Task Upsert_RowExists_ShouldUpdateSuccessfully()
        {
            var entity = new TableEntity
            {
                PartitionKey = DefaultPartitionKey,
                RowKey = DefaultRowKey,
                Value = 101010
            };

            var result = await this.repositoryUnderTest.Query<TableEntity>(entity.PartitionKey, entity.RowKey);
            result.Should().NotBeNull();
            result.Value.Should().Be(DefaultEntity.Value);

            await this.repositoryUnderTest.Upsert(entity);

            result = await this.repositoryUnderTest.Query<TableEntity>(entity.PartitionKey, entity.RowKey);
            result.Should().Be(entity);
        }

        private class TableEntity : ITableEntity
        {
            public string PartitionKey { get; init; }

            public string RowKey { get; init; }

            public int Value { get; init; }
        }
    }
}
