using System.Globalization;
using System.Text;

namespace MetersApp.Shared.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Converts a snake_case string to PascalCase.
    /// </summary>
    public static string SnakeToPascalCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Input cannot be null or whitespace.", nameof(input));
        }

        var words = input.Split('_', StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();

        foreach (var word in words)
        {
            sb.Append(char.ToUpper(word[0], CultureInfo.InvariantCulture));

            if (word.Length > 1)
            {
                sb.Append(word[1..].ToLower(CultureInfo.InvariantCulture));
            }
        }

        return sb.ToString();
    }
}
