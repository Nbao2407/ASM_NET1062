namespace ASM.DTOs;

public class FoodItemResponse
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
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
