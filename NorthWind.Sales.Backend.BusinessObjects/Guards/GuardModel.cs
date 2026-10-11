namespace NorthWind.Sales.Backend.BusinessObjects.Guards;
public static class GuardModel
{
 public static async Task AgainstNotValid<T>(IModelValidatorHub<T> hub,T model)
 { if (!await hub.Validate(model)) throw new ValidationException(hub.Errors); }
}
