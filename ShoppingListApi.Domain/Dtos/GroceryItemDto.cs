namespace ShoppingListApi.Domain.Dtos;

public class GroceryItemDto : IEquatable<GroceryItemDto>
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

    public bool Equals(GroceryItemDto? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && Name == other.Name && IsComplete == other.IsComplete;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((GroceryItemDto) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, IsComplete);
    }
}
