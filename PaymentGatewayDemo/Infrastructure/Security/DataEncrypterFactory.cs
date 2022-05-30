namespace PaymentGatewayDemo.Infrastructure.Security;

public class DataEncrypterFactory : IDataEncrypterFactory
{
    public IDataEncrypter CreateEncrypter(string encryptionKey)
    {
        return new DataEncrypter(encryptionKey);
    }
}
