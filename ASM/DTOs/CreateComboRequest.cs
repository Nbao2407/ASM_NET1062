using System.ComponentModel.DataAnnotations;
using ASM.Validation;

namespace ASM.DTOs;

public class CreateComboRequest
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(255)]
    [NoHtml]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    [NoHtml]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99")]
    public decimal Price { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "At least one combo item is required")]
    [MinLength(1, ErrorMessage = "At least one combo item is required")]
    public List<ComboItemRequest> ComboItems { get; set; } = new();
}

public class ComboItemRequest
{
    [Required(ErrorMessage = "FoodItemId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "FoodItemId must be a positive number")]
    public int FoodItemId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
    public int Quantity { get; set; } = 1;
}
