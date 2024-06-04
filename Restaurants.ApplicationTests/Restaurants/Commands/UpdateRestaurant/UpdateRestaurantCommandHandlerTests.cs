using Xunit;
using AutoMapper;
using Moq;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Constants;
using FluentAssertions;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();

        _handler = new UpdateRestaurantCommandHandler(
            _restaurantsRepositoryMock.Object,
            _loggerMock.Object,
            _restaurantAuthorizationServiceMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldUpdateRestaurants()
    {
        // Arrange
        var command = new UpdateRestaurantCommand
        {
            Id = 1,
            Name = "Test",
            Category = "Italian",
            HasDelivery = true,
        };

        var restaurant = new Restaurant
        {
            Id = 1,
            Name = "Old Name",
            Category = "Old Category",
            HasDelivery = false,
        };

        _restaurantsRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(x => x.Authorize(restaurant, ResourceOperation.Update))
            .Returns(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
        _restaurantsRepositoryMock.Verify(r => r.UpdateAsync(restaurant), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateRestaurantCommand
        {
            Id = 1,
            Name = "Test",
            Category = "Italian",
            HasDelivery = true,
        };
        _restaurantsRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync(value: null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        _restaurantsRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Restaurant>()), Times.Never);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_WithUnauthorizedUser_ShouldThrowForbidException()
    {
        // Arrange
        var command = new UpdateRestaurantCommand
        {
            Id = 1,
            Name = "Test",
            Category = "Italian",
            HasDelivery = true,
        };

        var restaurant = new Restaurant
        {
            Id = 1,
            Name = "Old Name",
            Category = "Old Category",
            HasDelivery = false,
        };

        _restaurantsRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
          .ReturnsAsync(restaurant);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        _restaurantsRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Restaurant>()), Times.Never);
        await act.Should().ThrowAsync<ForbidException>();
    }
}