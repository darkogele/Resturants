using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<Restaurant>> GetAllMatchingAsync(string? search, int pageSize, int pageNumber)
    {
        var searchLower = search?.ToLower();

        return await dbContext.Restaurants.
            Where(x =>  searchLower == null || 
                        (x.Name.ToLower().Contains(searchLower) || x.Description.ToLower().Contains(searchLower)))
            .Skip(pageSize * (pageSize -1))
            .Take(pageSize)
            .ToListAsync();
        
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