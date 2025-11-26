using System.ComponentModel.DataAnnotations;
using ASM.Validation;

namespace ASM.DTOs;

public class UpdateFoodItemRequest
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

    [Required(ErrorMessage = "Category is required")]
    [MaxLength(100)]
    [NoHtml]
    public string Category { get; set; } = string.Empty;

    [MaxLength(100)]
    [NoHtml]
    public string? Theme { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [MaxLength(1000)]
    [NoHtml]
    public string? Ingredients { get; set; }

    [MaxLength(1000)]
    [NoHtml]
    public string? NutritionalInfo { get; set; }

    [MaxLength(500)]
    [NoHtml]
    public string? AllergenWarnings { get; set; }

    public bool IsActive { get; set; } = true;
}
