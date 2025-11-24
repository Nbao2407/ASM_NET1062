namespace ASM.Models;

public class Combo
{
    public int ComboId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<ComboItem> ComboItems { get; set; } = new List<ComboItem>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
