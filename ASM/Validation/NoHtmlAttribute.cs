using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ASM.Validation;

public class NoHtmlAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string str)
        {
            if (Regex.IsMatch(str, "<.*?>"))
            {
                return new ValidationResult("HTML tags are not allowed.");
            }
        }
        return ValidationResult.Success;
    }
}
