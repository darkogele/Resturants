using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Persistence;

internal class RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options) : DbContext(options)
{
    internal DbSet<Restaurant> Restaurants { get; set; }
    internal DbSet<Dish> Dishes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>()
            .OwnsOne(x => x.Address)
            .ToTable("Restaurants");

        modelBuilder.Entity<Restaurant>()
            .HasMany(x => x.Dishes)
            .WithOne()
            .HasForeignKey(x => x.RestaurantId);
    }
}