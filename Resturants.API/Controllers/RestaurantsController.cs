using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;

namespace Restaurants.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantsController(IRestaurantsService restaurantsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await restaurantsService.GetAllRestaurants();
        return Ok(restaurants);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var restaurantById = await restaurantsService.GetRestaurantById(id);
        return restaurantById is null ? NotFound() : Ok(restaurantById);
    }
}