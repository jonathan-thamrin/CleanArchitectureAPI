using Microsoft.EntityFrameworkCore;
using ShoppingListApi.Domain.Models;

namespace ShoppingListApi.Application
{
    public interface IShoppingListContext
    {
        DbSet<GroceryItem> GroceryItems { get; set; }
        Task<int> SaveChangesAsync();
    }
}

