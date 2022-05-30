namespace PaymentGatewayDemo.Application.Services;

/// <summary>
/// Service for handling all user authorization.
/// </summary>
public interface IUserAuthorizationService
{
    /// <summary>
    /// Determines whether the calling user is authorized to perform the request.
    /// </summary>
    /// <param name="request">The request being performed.</param>
    /// <param name="merchantId">The ID of the merchant the action is being performed for.</param>
    /// <returns><c>True</c> if the user is authorized, else <c>false</c>.</returns>
    /// <remarks>
    /// Expansions for this include utilizing access tokens,
    /// and verifying the calling user's role (e.g. able to create payments or just view the transaction history).
    /// </remarks>
    Task VerifyAuthorization(HttpRequest request, string merchantId);
}
