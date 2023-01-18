using Microsoft.EntityFrameworkCore;
using ShoppingListApi.Application;
using ShoppingListApi.Application.Exceptions;
using ShoppingListApi.Domain.Dtos;
using ShoppingListApi.Domain.Models;

namespace ShoppingListApi.Infrastructure;

public class ShoppingListRepository : IShoppingListRepository
{
    private readonly IShoppingListContext context;

    public ShoppingListRepository(IShoppingListContext context)
    {
        this.context = context;
    }

    public async Task<List<GroceryItemDto>> GetGroceryItems()
    {
        return await context.GroceryItems.Select(groceryItem => ToDto(groceryItem)).ToListAsync();
    }

    public async Task<GroceryItemDto> GetGroceryItem(long id)
    {
        var groceryItem = await context.GroceryItems.FindAsync(id);

        if (groceryItem != null)
        {
            return ToDto(groceryItem);
        }

        throw new NotFoundException();
    }

    public async Task UpdateGroceryItem(long id, GroceryItemDto groceryItemDto)
    {
        if (id != groceryItemDto.Id)
        {
            throw new BadRequestException();
        }

        var groceryItem = await context.GroceryItems.FindAsync(id);

        if (groceryItem == null)
        {
            throw new NotFoundException();
        }

        groceryItem.Name = groceryItemDto.Name;
        groceryItem.IsComplete = groceryItemDto.IsComplete;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GroceryItemExists(id))
            {
                throw new NotFoundException();
            }

            throw;
        }
    }

    public async Task<GroceryItemDto> CreateGroceryItem(GroceryItemDto groceryItemDto)
    {
        var groceryItem = new GroceryItem
        {
            Name = groceryItemDto.Name,
            IsComplete = groceryItemDto.IsComplete,
        };

        context.GroceryItems.Add(groceryItem);
        await context.SaveChangesAsync();

        return ToDto(groceryItem);
    }

    public async Task DeleteGroceryItem(long id)
    {
        var groceryItem = await context.GroceryItems.FindAsync(id);

        if (groceryItem == null) throw new NotFoundException();
        
        context.GroceryItems.Remove(groceryItem);
        await context.SaveChangesAsync();
    }

    private static GroceryItemDto ToDto(GroceryItem? groceryItem)
    {
        return new GroceryItemDto
        {
            Id = groceryItem!.Id,
            Name = groceryItem.Name,
            IsComplete = groceryItem.IsComplete
        };
    }

    private bool GroceryItemExists(long id)
    {
        return (context.GroceryItems?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
