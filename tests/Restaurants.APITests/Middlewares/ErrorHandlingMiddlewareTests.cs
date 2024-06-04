using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace Restaurants.API.Middlewares.Tests;

public class ErrorHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var nextDelegateMock = new Mock<RequestDelegate>();

        // Act
        await middleware.InvokeAsync(context, nextDelegateMock.Object);

        // Assert
        nextDelegateMock.Verify(next => next(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var notFoundException = new NotFoundException(nameof(Restaurant), "1");

        // Act
        await middleware.InvokeAsync(context, _ => throw notFoundException);

        // Assert
        context.Response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusCode403()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var exception = new ForbidException();

        // Act
        await middleware.InvokeAsync(context, _ => throw exception);

        // Assert
        context.Response.StatusCode.Should().Be(403);
    }

    [Fact]
    public async Task InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCode500()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var exception = new Exception();

        // Act
        await middleware.InvokeAsync(context, _ => throw exception);

        // Assert
        context.Response.StatusCode.Should().Be(500);
    }
}