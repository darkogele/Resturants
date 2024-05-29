using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class CreatedMultipleRestaurantsRequirementHandlerI(
    IRestaurantsRepository restaurantsRepository,
    ILogger<CreatedMultipleRestaurantsRequirementHandlerI> logger,
    IUserContext userContext)
    : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        CreatedMultipleRestaurantsRequirement requirement)
    {
        var currentUser = userContext.CurrentUser();
        
        var restaurants = await restaurantsRepository.GetAllAsync();

        var userRestaurants = restaurants.Count(r => r.OwnerId == currentUser!.Id);

        if (userRestaurants >= requirement.MinimumRestaurantsCreated)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}