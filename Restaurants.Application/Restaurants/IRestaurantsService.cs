using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants;

public interface IRestaurantsService
{
    Task<int> CreateRestaurant(CreateRestaurantDto createRestaurantDto);
    Task<IEnumerable<RestaurantDto>> GetAllRestaurants();
    Task<RestaurantDto?> GetRestaurantById(int id);
}