using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await mediator.Send(new GetAllRestaurantsQuery());
        return Ok(restaurants);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var restaurantById = await mediator.Send(new GetRestaurantByIdQuery(id));
        return restaurantById is null ? NotFound() : Ok(restaurantById);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(CreateRestaurantCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateRestaurant(int id, UpdateRestaurantCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id in the URL and in the body do not match or are missing.");
        var isUpdated = await mediator.Send(command);
        return isUpdated is false ? NotFound() : NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRestaurant(int id)
    {
        var isDeleted = await mediator.Send(new DeleteRestaurantCommand(id));
        return isDeleted is false ? NotFound() : NoContent();
    }
}