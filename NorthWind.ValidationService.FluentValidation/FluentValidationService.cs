namespace NorthWind.ValidationService.FluentValidation;

internal sealed class FluentValidationService<T> : IValidationService<T>
{
    internal readonly InlineValidator<T> Validator = new();
    public IValidationRules<T,TProperty> AddRuleFor<TProperty>(Expression<Func<T,TProperty>> expression) => new ValidationRules<T,TProperty>(Validator.RuleFor(expression));
    public ICollectionValidationRules<T,TProperty> AddRuleForEach<TProperty>(Expression<Func<T,IEnumerable<TProperty>>> expression) => new CollectionValidationRules<T,TProperty>(Validator.RuleForEach(expression));
    public async Task<IEnumerable<ValidationError>> Validate(T model)
    {
        var result = await Validator.ValidateAsync(model);
        return result.Errors.Select(e => new ValidationError(e.PropertyName,e.ErrorMessage)).ToArray();
    }
}

internal sealed class ValidationRules<T,TProperty>(IRuleBuilderInitial<T,TProperty> builder) : IValidationRules<T,TProperty>
{
    public IValidationRules<T,TProperty> NotEmpty(string m) { builder.NotEmpty().WithMessage(m); return this; }
    public IValidationRules<T,TProperty> NotNull(string m) { builder.NotNull().WithMessage(m); return this; }
    public IValidationRules<T,TProperty> Must(Func<TProperty,bool> p,string m) { builder.Must(p).WithMessage(m); return this; }
    public IValidationRules<T,TProperty> StopOnFirstFailure() { builder.Cascade(CascadeMode.Stop); return this; }
    public IValidationRules<T,TProperty> GreaterThan<TValue>(TValue v,string m) where TValue:TProperty,IComparable<TValue>,IComparable { ((IRuleBuilder<T,TValue>)builder).GreaterThan(v).WithMessage(m); return this; }
    public IValidationRules<T,TProperty> Equal(Expression<Func<T,TProperty>> e,string m) { builder.Equal(e).WithMessage(m); return this; }
    IRuleBuilder<T,string> StringBuilder => (IRuleBuilder<T,string>)(object)builder;
    IValidationRules<T,string> StringRules => (IValidationRules<T,string>)(object)this;
    public IValidationRules<T,string> Length(int n,string m) { StringBuilder.Length(n).WithMessage(m); return StringRules; }
    public IValidationRules<T,string> MaximumLength(int n,string m) { StringBuilder.MaximumLength(n).WithMessage(m); return StringRules; }
    public IValidationRules<T,string> MinimumLength(int n,string m) { StringBuilder.MinimumLength(n).WithMessage(m); return StringRules; }
    public IValidationRules<T,string> EmailAddress(string m) { StringBuilder.EmailAddress().WithMessage(m); return StringRules; }
}

internal sealed class CollectionValidationRules<T,TProperty>(IRuleBuilderInitialCollection<T,TProperty> builder) : ICollectionValidationRules<T,TProperty>
{
    public ICollectionValidationRules<T,TProperty> SetValidator(IModelValidator<TProperty> validator)
    {
        if (validator is not AbstractModelValidator<TProperty> abstractValidator || abstractValidator.ValidationService is not FluentValidationService<TProperty> service)
            throw new InvalidOperationException("Collection validators must use this validation service implementation.");
        builder.SetValidator(service.Validator);
        return this;
    }
}

internal sealed class ModelValidatorHub<T>(IEnumerable<IModelValidator<T>> validators) : IModelValidatorHub<T>
{
    public IEnumerable<ValidationError> Errors { get; private set; } = [];
    public async Task<bool> Validate(T model)
    {
        var errors = new List<ValidationError>();
        foreach (var validator in validators.OrderBy(v => v.Constraint))
        {
            if (validator.Constraint == ValidationConstraint.AlwaysValidate || errors.Count == 0)
                if (!await validator.Validate(model)) errors.AddRange(validator.Errors);
        }
        Errors = errors;
        return errors.Count == 0;
    }
}

public abstract class AbstractModelValidator<T> : IModelValidator<T>
{
    private readonly IValidationService<T> validationService;
    protected AbstractModelValidator(IValidationService<T> validationService, ValidationConstraint constraint = ValidationConstraint.AlwaysValidate)
    { this.validationService = validationService; Constraint = constraint; }
    public ValidationConstraint Constraint { get; }
    public IEnumerable<ValidationError> Errors { get; private set; } = [];
    internal IValidationService<T> ValidationService => validationService;
    public async Task<bool> Validate(T model) { Errors = await validationService.Validate(model); return !Errors.Any(); }
    protected IValidationRules<T,TProperty> AddRuleFor<TProperty>(Expression<Func<T,TProperty>> expression) => validationService.AddRuleFor(expression);
    protected ICollectionValidationRules<T,TProperty> AddRuleForEach<TProperty>(Expression<Func<T,IEnumerable<TProperty>>> expression) => validationService.AddRuleForEach(expression);
}
