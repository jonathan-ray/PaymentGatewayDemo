namespace PaymentGatewayDemo.Infrastructure.Security;

/// <summary>
/// Abstraction of symmetrical data encryption logic.
/// </summary>
public interface IDataEncrypter
{
    // TODO: This should really be taking in a generic type and encrypting into/decrypting out of a byte array.

    /// <summary>
    /// Encrypt the given input.
    /// </summary>
    /// <param name="input">The input value.</param>
    /// <returns>The encrypted input value.</returns>
    public string Encrypt(string input);

    /// <summary>
    /// Decrypt the given input.
    /// </summary>
    /// <param name="input">The input value.</param>
    /// <returns>The decrypted input value.</returns>
    public string Decrypt(string input);
}
