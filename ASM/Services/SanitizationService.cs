using System.Text.RegularExpressions;

namespace ASM.Services;

public interface ISanitizationService
{
    string Sanitize(string input);
}

public class SanitizationService : ISanitizationService
{
    public string Sanitize(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        // Remove HTML tags
        return Regex.Replace(input, "<.*?>", string.Empty);
    }
}
