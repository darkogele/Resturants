using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(
    IRestaurantsRepository restaurantsRepository,
    ILogger<GetAllRestaurantsQueryHandler> logger,
    IMapper mapper) : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDto>>
{
    public async Task<IEnumerable<RestaurantDto>> Handle(GetAllRestaurantsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants");

        var restaurants = await restaurantsRepository.GetAllMatchingAsync(
            request.search,
            request.PageSize,
            request.PageNumber);
        
        //return restaurants.Select(RestaurantDto.FromEntity)!;
        return mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
    }
}