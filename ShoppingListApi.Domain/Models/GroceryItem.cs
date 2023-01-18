using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ShoppingListApi.Domain.Models;

[Table("GroceryItems")]
[Comment("Represents a user's shopping list")]
public class GroceryItem
{
    [Key]
    [Column("GroceryItem_Id")]
    public long Id { get; set; }
    [Required]
    [MinLength(1)]
    public string? Name { get; set; }
    [Required]
    public bool IsComplete { get; set; }
    public string? Author { get; set; }
}
