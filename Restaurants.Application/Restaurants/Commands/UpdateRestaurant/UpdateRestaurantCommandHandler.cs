using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    IRestaurantsRepository restaurantsRepository,
    ILogger<UpdateRestaurantCommandHandler> logger,
    IMapper mapper) : IRequestHandler<UpdateRestaurantCommand, bool>
{
    public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        if (restaurant is null)
        {
            logger.LogWarning("Restaurant with id {Id} was not found", request.Id);
            return false;
        }

        logger.LogInformation("Updating restaurant with id {Id}", request.Id);

        mapper.Map(request, restaurant);

        await restaurantsRepository.UpdateAsync(restaurant);
        return true;
    }
}