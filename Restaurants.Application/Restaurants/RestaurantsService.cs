﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(
    IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsService> logger,
    IMapper mapper) : IRestaurantsService
{
    public async Task<int> CreateRestaurant(CreateRestaurantDto createRestaurantDto)
    {
        logger.LogInformation("Creating a new restaurant: {name}", createRestaurantDto.Name);
        var restaurant = mapper.Map<Restaurant>(createRestaurantDto);

        var id = await restaurantsRepository.CreateAsync(restaurant);
        return id;
    }

    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();

        //return restaurants.Select(RestaurantDto.FromEntity)!;

        return mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
    }

    public async Task<RestaurantDto?> GetRestaurantById(int id)
    {
        logger.LogInformation("Getting restaurant by id: {id}", id);
        var restaurant = await restaurantsRepository.GetByIdAsync(id);

        //return RestaurantDto.FromEntity(restaurant);
        return mapper.Map<RestaurantDto?>(restaurant);
    }
}