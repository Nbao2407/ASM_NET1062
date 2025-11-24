using System.ComponentModel.DataAnnotations;

namespace ASM.DTOs;

public class UpdateComboRequest
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99")]
    public decimal Price { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "At least one combo item is required")]
    [MinLength(1, ErrorMessage = "At least one combo item is required")]
    public List<ComboItemRequest> ComboItems { get; set; } = new();
}
