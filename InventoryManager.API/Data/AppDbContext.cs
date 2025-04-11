using InventoryManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
}