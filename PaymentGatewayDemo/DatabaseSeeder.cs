using PaymentGatewayDemo.Infrastructure.Repositories.Entities;

namespace PaymentGatewayDemo
{
    /// <summary>
    /// Initial data to seed the database with. Use the given merchant ID to make calls to the API.
    /// </summary>
    public class DatabaseSeeder
    {
        private const string MerchantId = "fabrefactory";

        public static readonly IDictionary<string, IDictionary<string, ITableEntity>> SeedValue = new Dictionary<string, IDictionary<string, ITableEntity>>(StringComparer.OrdinalIgnoreCase)
        {
            {
                MerchantId,
                new Dictionary<string, ITableEntity>(StringComparer.OrdinalIgnoreCase)
                {
                    {
                        MerchantDetailsEntity.GetRowKey(),
                        new MerchantDetailsEntity
                        {
                            MerchantId = MerchantId,
                            ApiKey = "abc123",
                            AcquiringBank = Guid.NewGuid(),
                            MerchantBankId = $"{MerchantId}-Bank-ID"
                        }
                    }
                }
            }
        };
    }
}
