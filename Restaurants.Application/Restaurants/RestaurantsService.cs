using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantsRepository restaurantsRepository, ILogger<RestaurantsService> logger)
    : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();

        return restaurants.Select(RestaurantDto.FromEntity)!;
    }

    public async Task<RestaurantDto?> GetRestaurantById(int id)
    {
        logger.LogInformation("Getting restaurant by id: {id}", id);
        var restaurant = await restaurantsRepository.GetByIdAsync(id);

        return RestaurantDto.FromEntity(restaurant);
    }
}