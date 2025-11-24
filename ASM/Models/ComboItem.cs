namespace ASM.Models;

public class ComboItem
{
    public int ComboItemId { get; set; }
    public int ComboId { get; set; }
    public int FoodItemId { get; set; }
    public int Quantity { get; set; } = 1;
    
    public Combo Combo { get; set; } = null!;
    public FoodItem FoodItem { get; set; } = null!;
}
