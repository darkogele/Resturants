using AutoMapper;

namespace Restaurants.Application.Dishes.Dtos;

public class DishesProfile : Profile
{
    public DishesProfile()
    {
        CreateMap<Domain.Entities.Dish, DishDto>();
    }
}