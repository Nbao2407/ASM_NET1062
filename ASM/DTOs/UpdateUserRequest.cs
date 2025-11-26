using System.ComponentModel.DataAnnotations;
using ASM.Validation;

namespace ASM.DTOs;

public class UpdateUserRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Full name is required")]
    [MaxLength(255)]
    [NoHtml]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(500)]
    [NoHtml]
    public string? Address { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Role is required")]
    [RegularExpression("^(Customer|Admin)$", ErrorMessage = "Role must be either 'Customer' or 'Admin'")]
    public string Role { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
