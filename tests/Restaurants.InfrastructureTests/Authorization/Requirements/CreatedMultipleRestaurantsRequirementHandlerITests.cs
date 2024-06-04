using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests;

public class CreatedMultipleRestaurantsRequirementHandlerITests
{
    [Fact]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        // Arrange
        var cuurentUuer = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(u => u.CurrentUser()).Returns(cuurentUuer);

        var restaurants = new List<Restaurant>
        {
            new() { OwnerId = cuurentUuer.Id },
            new() { OwnerId = cuurentUuer.Id },
            new() { OwnerId = cuurentUuer.Id }
        };
        
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsRequirement(2);
        var handler = new CreatedMultipleRestaurantsRequirementHandler(restaurantsRepositoryMock.Object,
            userContextMock.Object);
        var context = new AuthorizationHandlerContext([requirement], null, null);
        
        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(m => m.CurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = "2",
                },
            };

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsRequirement(2);
        var handler = new CreatedMultipleRestaurantsRequirementHandler(restaurantsRepositoryMock.Object,
            userContextMock.Object);
        var context = new AuthorizationHandlerContext([requirement], null, null);

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();
    }
}