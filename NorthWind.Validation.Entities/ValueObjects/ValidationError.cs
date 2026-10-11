namespace NorthWind.Validation.Entities.ValueObjects;
public sealed class ValidationError(string propertyName, string message)
{
    public string PropertyName { get; } = propertyName;
    public string Message { get; } = message;
}
