namespace NorthWind.ValidationService.FluentValidation;
public abstract class AbstractViewModelValidator<TDto,TViewModel>(IModelValidatorHub<TDto> hub, ValidationConstraint constraint) : IModelValidator<TViewModel>
{
 public ValidationConstraint Constraint { get; } = constraint;
 public IEnumerable<ValidationError> Errors => hub.Errors;
 public virtual TDto Cast(TViewModel model)
 {
  var method=typeof(TViewModel).GetMethod("op_Explicit",System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.Static);
  if(method is null) throw new InvalidCastException($"{typeof(TViewModel).Name} must define an explicit conversion to {typeof(TDto).Name} or override Cast.");
  return (TDto)method.Invoke(null,[model])!;
 }
 public Task<bool> Validate(TViewModel model) => hub.Validate(Cast(model));
}
