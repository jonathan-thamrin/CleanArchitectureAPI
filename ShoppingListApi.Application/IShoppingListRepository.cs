using ShoppingListApi.Domain.Dtos;

namespace ShoppingListApi.Application;

public interface IShoppingListRepository
{
    Task<List<GroceryItemDto>> GetGroceryItems();
    Task<GroceryItemDto> GetGroceryItem(long id);
    Task UpdateGroceryItem(long id, GroceryItemDto groceryItemDto);
    Task<GroceryItemDto> CreateGroceryItem(GroceryItemDto groceryItemDto);
    Task DeleteGroceryItem(long id);
}
