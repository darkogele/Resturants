﻿using AutoMapper;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantProfile : Profile
{
    public RestaurantProfile()
    {
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(d => d.City, opt =>
                 opt.MapFrom(src => src.Address == null ? null : src.Address.City))
                     .ForMember(d => d.PostalCode, opt =>
                 opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
                      .ForMember(d => d.Street, opt =>
                 opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
            .ForMember(d => d.Dishes, opt => opt.MapFrom(src => src.Dishes));

        CreateMap<CreateRestaurantDto, Restaurant>()
            .ForMember(d => d.Address, opt => opt.MapFrom(
                src => new Address
                {
                    Street = src.Street,
                    City = src.City,
                    PostalCode = src.PostalCode
                }));
    }
}