using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDish;

internal class DeleteDishCommandHandler(
    ILogger<DeleteDishCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<DeleteDishCommand>
{
    public async Task Handle(DeleteDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting dish {DishId} for restaurant {RestaurantId}",
            request.DishId, request.RestaurantId);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
                         ??
                         throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId)
                   ??
                   throw new NotFoundException(nameof(Dish), request.DishId.ToString());

        restaurant.Dishes.Remove(dish);
        await restaurantsRepository.UpdateAsync(restaurant);
    }
}