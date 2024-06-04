using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDIshCommandHandler(
    ILogger<CreateDIshCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService, 
    IMapper mapper) : IRequestHandler<CreateDishCommand, int>
{
    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new dish {@CreateDishCommand}", request);
        
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ??
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        
        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Create))
            throw new ForbidException();
        
        var dish = mapper.Map<Dish>(request);
        return await dishesRepository.CreateAsync(dish);
    }
}