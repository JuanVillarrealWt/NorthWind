namespace NorthWind.RazorComponents.Formatting;

public static class TextInputNormalizer
{
    public static string KeepLettersAndSpacesOnly(string? value) =>
        new((value ?? string.Empty).Where(character => char.IsLetter(character) || character == ' ').ToArray());

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
        var normalized = (value ?? string.Empty).TrimStart();
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
}
