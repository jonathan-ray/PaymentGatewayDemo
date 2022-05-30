using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using PaymentGatewayDemo.Infrastructure.Exceptions;
using PaymentGatewayDemo.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace PaymentGatewayDemo.Tests.UnitTests.Infrastructure.Filters
{
    [Trait("Category", "UnitTests")]
    public class HttpExceptionFilterTests
    {
        private readonly IActionFilter filterUnderTest;

        public HttpExceptionFilterTests()
        {
            this.filterUnderTest = new HttpExceptionFilter();
        }

        [Fact]
        public void OnActionExecuting_ShouldDoNothing()
        {
            var executingContext = new ActionExecutingContext(
                new ActionContext(
                    Mock.Of<HttpContext>(),
                    Mock.Of<RouteData>(),
                    Mock.Of<ActionDescriptor>(),
                    new ModelStateDictionary()),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>());

            this.filterUnderTest
                .Invoking(f => f.OnActionExecuting(executingContext))
                .Should().NotThrow();
        }

        [Fact]
        public void OnActionExecuted_WithoutException_ShouldDoNothing()
        {
            var executedContext = new ActionExecutedContext(
                new ActionContext(
                    Mock.Of<HttpContext>(),
                    Mock.Of<RouteData>(),
                    Mock.Of<ActionDescriptor>(),
                    new ModelStateDictionary()),
                new List<IFilterMetadata>(),
                Mock.Of<Controller>());

            var expectedResult = executedContext.Result;

            executedContext.Exception = null;

            this.filterUnderTest.OnActionExecuted(executedContext);

            executedContext.Result.Should().Be(expectedResult);
        }

        [Fact]
        public void OnActionExecuted_WithUnknownException_ShouldNotHandleException()
        {
            var executedContext = new ActionExecutedContext(
                new ActionContext(
                    Mock.Of<HttpContext>(),
                    Mock.Of<RouteData>(),
                    Mock.Of<ActionDescriptor>(),
                    new ModelStateDictionary()),
                new List<IFilterMetadata>(),
                Mock.Of<Controller>());

            var expectedResult = executedContext.Result;

            executedContext.Exception = new ArithmeticException();
            executedContext.ExceptionHandled = false;

            this.filterUnderTest.OnActionExecuted(executedContext);

            executedContext.Result.Should().Be(expectedResult);
            executedContext.ExceptionHandled.Should().BeFalse();
        }

        [Fact]
        public void OnActionExecuted_WithKnownException_ShouldNotHandleException()
        {
            var exceptionAndExpectedStatusCodes = new (Exception, HttpStatusCode)[]
            {
                (new AcquiringBankServiceException(new Exception()), HttpStatusCode.InternalServerError),
                (new AuthorizationFailureException("a1"), HttpStatusCode.Forbidden),
                (new IdempotentPaymentTransactionException("abc", "123"), HttpStatusCode.Conflict),
                (new MerchantNotFoundException("abc"), HttpStatusCode.NotFound),
                (new PaymentTransactionNotFoundException("abc", "123"), HttpStatusCode.NotFound)
            };

            foreach (var (exception, expectedStatusCode) in exceptionAndExpectedStatusCodes)
            {
                this.TestKnownExceptionScenario(exception, expectedStatusCode);
            }
        }

        private void TestKnownExceptionScenario(Exception exception, HttpStatusCode expectedStatusCode)
        {
            var executedContext = new ActionExecutedContext(
                new ActionContext(
                    Mock.Of<HttpContext>(),
                    Mock.Of<RouteData>(),
                    Mock.Of<ActionDescriptor>(),
                    new ModelStateDictionary()),
                new List<IFilterMetadata>(),
                Mock.Of<Controller>());

            executedContext.Exception = exception;
            executedContext.ExceptionHandled = false;

            this.filterUnderTest.OnActionExecuted(executedContext);

            executedContext.Result.Should().BeOfType<ObjectResult>();
            ((ObjectResult)executedContext.Result).StatusCode.Should().Be((int)expectedStatusCode);
            executedContext.ExceptionHandled.Should().BeTrue();
        }
    }
}
