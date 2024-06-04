using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger, IUserContext userContext)
    : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation operation)
    {
        var user = userContext.CurrentUser();

        logger.LogInformation("Auhorizing user {userEmail}, to {operation} restaurant {restaurantName}",
            user!.Email,
            operation,
            restaurant.Name);

        if (operation is ResourceOperation.Create or ResourceOperation.Read)
        {
            logger.LogInformation("Create/Read operation - successful authorization");
            return true;
        }

        if (operation is ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Admin use, delete operation r- successful authorization");
            return true;
        }

        if ((operation is ResourceOperation.Delete or ResourceOperation.Update) &&
            user.IsInRole(UserRoles.RestaurantOwner))
        {
            logger.LogInformation("Restaurant owner, delete/update operation - successful authorization");
            return true;
        }
        
        return false;
    }
}