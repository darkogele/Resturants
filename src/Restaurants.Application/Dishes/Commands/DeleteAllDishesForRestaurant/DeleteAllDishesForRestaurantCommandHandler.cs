using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteAllDishesForRestaurant;

internal class DeleteAllDishesForRestaurantCommandHandler(
    ILogger<DeleteAllDishesForRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService,
    IDishesRepository dishesRepository) : IRequestHandler<DeleteAllDishesForRestaurantCommand>
{
    public async Task Handle(DeleteAllDishesForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting all dishes for restaurant {RestaurantId}", request.RestaurantId);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
                         ??
                         throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbidException();

        await dishesRepository.DeleteAsync(restaurant.Dishes);
    }
}