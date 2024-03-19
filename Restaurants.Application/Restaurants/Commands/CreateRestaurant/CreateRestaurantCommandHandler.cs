using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    ILogger<CreateRestaurantCommandHandler> logger) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken ct)
    {
        logger.LogInformation("Creating a new restaurant: {Name}", request.Name);
        var restaurant = mapper.Map<Restaurant>(request);

        var id = await restaurantsRepository.CreateAsync(restaurant);
        return id;
    }
}