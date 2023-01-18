using Microsoft.AspNetCore.Mvc;
using ShoppingListApi.Application;
using ShoppingListApi.Application.Exceptions;
using ShoppingListApi.Domain.Dtos;

/*
 * Controllers: Summary
 *
 * 1. Controllers shouldn't contain too much business logic.
 * 2. They handle correct calls/routes are being made.
 * 3. It delegates logic outside to external functions.
 */

/*
 * Controller Testing: Summary
 *
 * 1. You should not be doing UNIT TESTS on Controllers. Controllers work best as a whole, i.e., as part of a framework,
 * in this case it is ASP.NET Core. You do not want to test the individual methods in isolation as it serves no useful
 * purpose.
 * 2. Instead, you could/should be UNIT TESTING the Repository context that the Controller uses.
 * It makes more sense to test CRUD operations are working on a given database context (i.e., in-memory for purposes of
 * tests).
 */

namespace ShoppingListApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListRepository repository;

        // Uses Dependency Injection to inject the database context into the controller.
        // Setup is in Program.cs @ Presentation layer.
        public ShoppingListController(IShoppingListRepository repository)
        {
            this.repository = repository;
        }

        // Actions are methods responsible for executing requests and generating responses for them.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroceryItemDto>>> GetGroceryItems()
        {
            return await repository.GetGroceryItems();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroceryItemDto>> GetGroceryItem(long id)
        {
            try
            {
                return await repository.GetGroceryItem(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // Request must contain the entire updated entity including "id" (not just the changes unlike PATCH).
        // Response is 204 NO CONTENT if update is successful.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroceryItem(long id, GroceryItemDto groceryItemDto)
        {
            try
            {
                await repository.UpdateGroceryItem(id, groceryItemDto);
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<GroceryItemDto>> PostGroceryItem(GroceryItemDto groceryItemDto)
        {
            var groceryItem = await repository.CreateGroceryItem(groceryItemDto);

            // Produces a 201 CREATED status code.
            // First Param: Action used to generate URI for the Location header in the response.
            // Second Param: Route data used for generating URL.
            // Third Param: Value of Content in the entity body.
            return CreatedAtAction(nameof(GetGroceryItem), new {id = groceryItem.Id}, groceryItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroceryItem(long id)
        {
            try
            {
                await repository.DeleteGroceryItem(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            
            return NoContent();
        }
    }
}
