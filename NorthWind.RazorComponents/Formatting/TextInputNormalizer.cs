namespace NorthWind.RazorComponents.Formatting;

public static class TextInputNormalizer
{
    public static string KeepDigitsOnly(string? value) =>
        new((value ?? string.Empty).Where(char.IsAsciiDigit).ToArray());

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
