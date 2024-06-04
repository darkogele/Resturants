using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

internal class CreatedMultipleRestaurantsRequirementHandler(
    IRestaurantsRepository restaurantsRepository,
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