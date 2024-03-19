using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;

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

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(CreateRestaurantDto createRestaurantDto)
    {
        var id = await restaurantsService.CreateRestaurant(createRestaurantDto);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }
}