using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishesRepository
{
    Task<int> CreateAsync(Dish dish);

    Task DeleteAsync(IEnumerable<Dish> dish);
}