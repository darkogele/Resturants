using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(
    IRestaurantsRepository restaurantsRepository,
    ILogger<GetRestaurantByIdQueryHandler> logger,
    IMapper mapper) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
{
    public async Task<RestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting restaurant by id: {Id}", request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id)
                         ??
                         throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        // return RestaurantDto.FromEntity(restaurant);
        return mapper.Map<RestaurantDto>(restaurant);
    }
}