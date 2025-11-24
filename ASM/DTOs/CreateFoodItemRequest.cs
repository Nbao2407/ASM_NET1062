using System.ComponentModel.DataAnnotations;

namespace ASM.DTOs;

public class CreateFoodItemRequest
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Theme { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [MaxLength(1000)]
    public string? Ingredients { get; set; }

    [MaxLength(1000)]
    public string? NutritionalInfo { get; set; }

    [MaxLength(500)]
    public string? AllergenWarnings { get; set; }
}
