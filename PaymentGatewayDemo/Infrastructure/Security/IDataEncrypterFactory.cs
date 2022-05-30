namespace PaymentGatewayDemo.Infrastructure.Security;

/// <summary>
/// Factory class for creating data encrypter instances.
/// </summary>
public interface IDataEncrypterFactory
{
    /// <summary>
    /// Create a data encrypter with a specific encryption key.
    /// </summary>
    /// <param name="encryptionKey">The encryption key.</param>
    /// <returns>The data encrypter.</returns>
    IDataEncrypter CreateEncrypter(string encryptionKey);
}
