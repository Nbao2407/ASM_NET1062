namespace ASM.Models;

public class FoodItem
{
    public int FoodItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Theme { get; set; }
    public string? ImageUrl { get; set; }
    public string? Ingredients { get; set; }
    public string? NutritionalInfo { get; set; }
    public string? AllergenWarnings { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<ComboItem> ComboItems { get; set; } = new List<ComboItem>();
}
