namespace PaymentGatewayDemo.Infrastructure.Security;

public class DataEncrypter : IDataEncrypter
{
    // TODO: This is a stubbed data encryption implementation.
    // This should use a symmetrical encryption algorithm (AES) to encrypt/decrypt the given values.

    private readonly string encryptionKey;

    public DataEncrypter(string encryptionKey)
    {
        this.encryptionKey = encryptionKey ?? throw new ArgumentNullException(nameof(encryptionKey));
    }

    public string Decrypt(string input)
    {
        return input;
    }

    public string Encrypt(string input)
    {
        return input;
    }
}
