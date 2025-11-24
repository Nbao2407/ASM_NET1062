using System.ComponentModel.DataAnnotations;

namespace ASM.DTOs;

public class GoogleAuthRequest
{
    [Required(ErrorMessage = "Google ID token is required")]
    public string IdToken { get; set; } = string.Empty;
}
