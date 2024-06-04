using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    ILogger<CreateRestaurantCommandHandler> logger,
    IUserContext userContext) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken ct)
    {
        var currentUser = userContext.CurrentUser();
        if (currentUser == null)
        {
            logger.LogWarning("Current user is null");
            throw new UnauthorizedAccessException("Current user is null");
        }
        
        logger.LogInformation("{UserEMail} [{UserId}] is creating a new restaurant: {@Restaurant}",
            currentUser.Email,
            currentUser.Id,
            request);
        
        var restaurant = mapper.Map<Restaurant>(request);
        restaurant.OwnerId = currentUser.Id;
        
        var id = await restaurantsRepository.CreateAsync(restaurant);
        return id;
    }
}