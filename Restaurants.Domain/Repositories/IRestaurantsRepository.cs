using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<Restaurant?> GetByIdAsync(int id);
    Task<int> CreateAsync(Restaurant restaurant);
    Task UpdateAsync(Restaurant restaurant);
    Task Delete(Restaurant restaurant);

    Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? search, int pageSize, int pageNumber,
        string? sortBy, SortDirection sortDirection);
}