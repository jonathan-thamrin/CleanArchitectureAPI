using Microsoft.EntityFrameworkCore;
using ShoppingListApi.Application;
using ShoppingListApi.Domain.Models;

namespace ShoppingListApi.Infrastructure;

public class ShoppingListContext : DbContext, IShoppingListContext
{
    public ShoppingListContext(DbContextOptions<ShoppingListContext> options) : base(options)
    {
    }
    
    public DbSet<GroceryItem> GroceryItems { get; set; }
    
    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }
}
