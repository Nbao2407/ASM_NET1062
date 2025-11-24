namespace ASM.DTOs;

public class ComboResponse
{
    public int ComboId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<ComboItemResponse> ComboItems { get; set; } = new();
}

public class ComboItemResponse
{
    public int FoodItemId { get; set; }
    public string FoodItemName { get; set; } = string.Empty;
    public decimal FoodItemPrice { get; set; }
    public int Quantity { get; set; }
}
