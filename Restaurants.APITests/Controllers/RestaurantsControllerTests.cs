using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.APITests;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();

    public RestaurantsControllerTests(WebApplicationFactory<Program> webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                // Replace the existing IRestaurantsRepository with the mock
                services.Replace(ServiceDescriptor.Scoped(sp => _restaurantsRepositoryMock.Object));

                // Same as above
                //services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository), 
                //    _ => _restaurantsRepositoryMock.Object));
            });
        });
    }

    [Fact]
    public async Task GetAll_ForValidRequest_Returns200Ok()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();

        // Act
        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();

        // Act
        var result = await client.GetAsync("/api/restaurants");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
    {
        // Arrange
        var id = 12345;

        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);

        var client = _webApplicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/restaurants/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_ForExistingId_ShouldReturn200Ok()
    {
        // Arrange
        var id = 99;

        var restaurant = new Restaurant()
        {
            Id = id,
            Name = "Test",
            Description = "Test description"
        };

        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(restaurant);

        var client = _webApplicationFactory.CreateClient();

        // act
        var response = await client.GetAsync($"/api/restaurants/{id}");
        var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto!.Name.Should().Be("Test");
        restaurantDto.Description.Should().Be("Test description");
    }

}