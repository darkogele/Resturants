using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    IRestaurantsRepository restaurantsRepository,
    ILogger<UpdateRestaurantCommandHandler> logger,
    IRestaurantAuthorizationService restaurantAuthorizationService,
    IMapper mapper) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        
        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbidException();
        
        logger.LogInformation("Updating restaurant with id {Id} with {@UpdateRestaurant}", request.Id, request);

        mapper.Map(request, restaurant);

        await restaurantsRepository.UpdateAsync(restaurant);
    }
}