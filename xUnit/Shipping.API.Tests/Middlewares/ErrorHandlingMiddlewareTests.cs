using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;

namespace Shipping.API.Middlewares.Tests;

public class ErrorHandlingMiddlewareTests
{
    [Fact()]
    public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
    {
        //Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var nextDelgateMock = new Mock<RequestDelegate>();

        //Act

        await middleware.InvokeAsync(context, nextDelgateMock.Object);

        //Assert
        nextDelgateMock.Verify(next => next.Invoke(context), Times.Once);

    }

    [Fact()]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
    {
        //arrange 

        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();

        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);

        var context = new DefaultHttpContext();

        var notFoundException = new NotFoundException(nameof(Order), "1");

        //act

        await middleware.InvokeAsync(context, _ => throw notFoundException);

        //assert

        context.Response.StatusCode.Should().Be(404);
    }

    [Fact()]
    public async Task InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCode500()
    {
        //arrange 

        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();

        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);

        var context = new DefaultHttpContext();

        var exception = new Exception();

        //act

        await middleware.InvokeAsync(context, _ => throw exception);

        //assert

        context.Response.StatusCode.Should().Be(500);
    }
}