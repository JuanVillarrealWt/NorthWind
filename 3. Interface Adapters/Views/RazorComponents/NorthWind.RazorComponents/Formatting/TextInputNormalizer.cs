using System.Text.RegularExpressions;

namespace NorthWind.RazorComponents.Formatting;

public static class TextInputNormalizer
{
    public static string CollapseConsecutiveSpaces(string? value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        return Regex.Replace(value, @"\s{2,}", " ");
    }

    public static string KeepLettersAndSpacesOnly(string? value)
    {
        var raw = value ?? string.Empty;
        var filtered = new string(raw.Where(character => char.IsLetter(character) || character == ' ').ToArray());
        return CollapseConsecutiveSpaces(filtered.TrimStart());
    }

    public static string KeepDigitsOnly(string? value) =>
        new((value ?? string.Empty).Where(char.IsAsciiDigit).ToArray());

    public static string NormalizeLeadingZeros(string? value)
    {
        var text = value ?? string.Empty;
        if (text.Length == 0)
        {
            return text;
        }

        var decimalPoint = text.IndexOf('.');
        var integerPart = decimalPoint < 0 ? text : text[..decimalPoint];
        var normalizedInteger = integerPart.TrimStart('0');
        if (normalizedInteger.Length == 0 && (integerPart.Length > 0 || decimalPoint >= 0))
        {
            normalizedInteger = "0";
        }

        return decimalPoint < 0
            ? normalizedInteger
            : $"{normalizedInteger}.{text[(decimalPoint + 1)..]}";
    }

    public static string NormalizeInitialCapital(string? value)
    {
        var normalized = CollapseConsecutiveSpaces((value ?? string.Empty).TrimStart());
        var firstLetterIndex = 0;
        while (firstLetterIndex < normalized.Length && !char.IsLetter(normalized[firstLetterIndex]))
        {
            firstLetterIndex++;
        }

        if (firstLetterIndex >= normalized.Length)
        {
            return normalized;
        }

        var characters = normalized.ToCharArray();
        characters[firstLetterIndex] = char.ToUpperInvariant(characters[firstLetterIndex]);
        return new string(characters);
    }

    public static string CleanBeforeSave(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;
        return CollapseConsecutiveSpaces(value.Trim());
    }
}
