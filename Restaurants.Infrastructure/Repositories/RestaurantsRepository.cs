using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    public async Task<int> CreateAsync(Restaurant restaurant)
    {
        await dbContext.Restaurants.AddAsync(restaurant);
        await dbContext.SaveChangesAsync();
        return restaurant.Id;
    }

    public async Task UpdateAsync(Restaurant restaurant)
    {
        dbContext.Restaurants.Update(restaurant);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(Restaurant restaurant)
    {
        dbContext.Remove(restaurant);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return await dbContext.Restaurants
            .Include(x => x.Dishes)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Restaurant>, int )> GetAllMatchingAsync(string? search, int pageSize, int pageNumber,
        string? sortBy, SortDirection sortDirection)
    {
        var searchLower = search?.ToLower();

        var baseQuery = dbContext.Restaurants.Where(x => searchLower == null ||
                                                         (x.Name.ToLower().Contains(searchLower) ||
                                                          x.Description.ToLower().Contains(searchLower)));

        var totalCount = await baseQuery.CountAsync();

        if (sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), r => r.Name },
                { nameof(Restaurant.Description), r => r.Description },
                { nameof(Restaurant.Category), r => r.Category },
            };

            Expression<Func<Restaurant, object>> selectedColumn = columnsSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }

        var restaurants = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (restaurants, totalCount);

        // PageSize = 5, PageNumber = 3 : Skip => PageSize * (pageNumber =1) => 5  *  (3-1) => 10

        // 1 {....}
        // 2 {....}
        // 3 {....}
        // 4 {....}
        // 5 {....}
        // 6 {....}
        // 7 {....}
        // 8 {....}
        // 9 {....}
        // 10 {....}
        // 11 {....}
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        return await dbContext.Restaurants
            .Include(x => x.Dishes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}