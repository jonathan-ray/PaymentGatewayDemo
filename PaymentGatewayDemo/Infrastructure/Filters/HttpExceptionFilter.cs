using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PaymentGatewayDemo.Infrastructure.Exceptions;
using System.Net;

namespace PaymentGatewayDemo.Infrastructure.Filters;

public class HttpExceptionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) 
    {
        // Nothing needed to be done on action executing.
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is not null)
        {
            // TODO: Add logging.

            HttpStatusCode? statusCode = context.Exception switch
            {
                AcquiringBankServiceException => HttpStatusCode.InternalServerError,
                AuthorizationFailureException => HttpStatusCode.Forbidden,
                IdempotentPaymentTransactionException => HttpStatusCode.Conflict,
                MerchantNotFoundException => HttpStatusCode.NotFound,
                PaymentTransactionNotFoundException => HttpStatusCode.NotFound,
                _ => null
            };

            if (statusCode is not null)
            {
                context.Result = new ObjectResult(context.Exception.Message)
                {
                    StatusCode = (int)statusCode
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
