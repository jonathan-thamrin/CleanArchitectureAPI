using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingListApi.Application;
using ShoppingListApi.Application.Exceptions;
using ShoppingListApi.Domain.Dtos;
using ShoppingListApi.Domain.Models;
using ShoppingListApi.Infrastructure;

namespace ShoppingListApi.Test.UnitTests;

public class ShoppingListRepositoryTest
{
    private readonly IShoppingListRepository repository;
    private readonly ShoppingListContext context;

    public ShoppingListRepositoryTest()
    {
        // Establishes a Database Provider. Must contain all service required by EF and database being used.
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        // Uses specified service provider. EF Core automatically creates one if none is given.
        var options = new DbContextOptionsBuilder<ShoppingListContext>()
            .UseInMemoryDatabase("testDb")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        context = new ShoppingListContext(options);
        
        context.GroceryItems.AddRange(new List<GroceryItem>
        {
            new()
            {
                Id = 1,
                Name = "Butter",
                IsComplete = false,
                Author = "Admin"
            },
            new()
            {
                Id = 2,
                Name = "Chocolate",
                IsComplete = true,
                Author = "Admin"
            }
        });

        context.SaveChangesAsync();

        repository = new ShoppingListRepository(context);
    }

    [Fact]
    public async void GivenARepository_WhenGettingAllGroceryItems_ThenShouldReturnAllGroceryItems()
    {
        // Arrange
        var expectedGroceryItems = new List<GroceryItemDto>
        {
            new()
            {
                Id = 1,
                Name = "Butter",
                IsComplete = false
            },
            new()
            {
                Id = 2,
                Name = "Chocolate",
                IsComplete = true
            }
        };

        // Act
        var actualGroceryItems = await repository.GetGroceryItems();

        // Assert
        Assert.Equal(expectedGroceryItems, actualGroceryItems);
    }

    [Theory]
    [InlineData(1, "Butter", false)]
    [InlineData(2, "Chocolate", true)]
    public async void GivenARepository_WhenGettingSingleGroceryItemById_ThenShouldReturnRespectiveGroceryItem(long id,
        string name, bool isComplete)
    {
        // Arrange
        var expectedGroceryItem = new GroceryItemDto
        {
            Id = id,
            Name = name,
            IsComplete = isComplete
        };

        // Act
        var actualGroceryItem = await repository.GetGroceryItem(id);

        // Assert
        Assert.Equal(expectedGroceryItem, actualGroceryItem);
    }

    [Fact]
    public async Task GivenARepository_WhenGettingGroceryItemByIdThatDoesNotExist_ThenShouldReturnNotFound()
    {
        // Arrange
        var id = 3;

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => repository.GetGroceryItem(id));
    }

    [Fact]
    public async void GivenARepository_WhenUpdatingGroceryItem_ThenShouldUpdateRespectiveGroceryItemInDatabase()
    {
        // Arrange
        var id = 1;
        var oldGroceryItemInDb = await repository.GetGroceryItem(id);
        var updatedGroceryItem = new GroceryItemDto()
        {
            Id = id,
            Name = "Milk",
            IsComplete = false
        };

        // Act
        await repository.UpdateGroceryItem(id, updatedGroceryItem);
        var newGroceryItemInDb = await repository.GetGroceryItem(id);

        // Assert
        Assert.Equal(updatedGroceryItem, newGroceryItemInDb);
        Assert.NotEqual(oldGroceryItemInDb, newGroceryItemInDb);
    }

    [Fact]
    public void GivenARepository_WhenUpdatingGroceryItemWithMismatchingId_ThenThrowBadRequest()
    {
        // Arrange
        var mismatchedId = 1;
        var updatedGroceryItem = new GroceryItemDto()
        {
            Id = 2,
            Name = "Milk",
            IsComplete = false
        };

        // Assert
        Assert.ThrowsAsync<BadRequestException>(() => repository.UpdateGroceryItem(mismatchedId, updatedGroceryItem));
    }

    [Fact]
    public void GivenARepository_WhenUpdatingGroceryItemThatDoesNotExist_ThenThrowNotFound()
    {
        // Arrange
        var id = 5;
        var updatedGroceryItem = new GroceryItemDto()
        {
            Id = id,
            Name = "Milk",
            IsComplete = false
        };

        // Assert
        Assert.ThrowsAsync<NotFoundException>(() => repository.UpdateGroceryItem(id, updatedGroceryItem));
    }

    [Fact]
    public async void GivenARepository_WhenAddingNewGroceryItem_ThenShouldAddEntryToDatabase()
    {
        // Arrange
        var noOfItemsInContext = repository.GetGroceryItems().Result.Count;
        var expectedGroceryItem = new GroceryItemDto()
        {
            Id = noOfItemsInContext + 1,
            Name = "Cereal",
            IsComplete = false
        };

        // Act
        await repository.CreateGroceryItem(expectedGroceryItem);
        var actualGroceryItem = await repository.GetGroceryItems();

        // Assert
        Assert.Equal(expectedGroceryItem, actualGroceryItem.Last());
    }

    [Fact]
    public async void GivenARepository_WhenDeletingExistingGroceryItem_ThenRemovesEntryFromDatabase()
    {
        // Arrange
        var groceryItemIdToDelete = 1;
        var groceryItemsBeforeDeletion = await repository.GetGroceryItems();

        // Act
        await repository.DeleteGroceryItem(groceryItemIdToDelete);
        var groceryItemsAfterDeletion = await repository.GetGroceryItems();

        // Assert
        Assert.NotEqual(groceryItemsBeforeDeletion, groceryItemsAfterDeletion);
    }

    [Fact]
    public void GivenARepository_WhenDeletingNonExistingGroceryItem_ThenThrowNotFound()
    {
        // Arrange
        var groceryItemIdToDelete = 5;

        // Assert
        Assert.ThrowsAsync<NotFoundException>(() => repository.DeleteGroceryItem(groceryItemIdToDelete));
    }
}
