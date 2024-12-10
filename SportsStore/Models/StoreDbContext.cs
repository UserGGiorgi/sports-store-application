using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

public class StoreDbContext : DbContext
{
    public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => this.Set<Product>();
    
    public DbSet<Order> Orders => this.Set<Order>();
}
