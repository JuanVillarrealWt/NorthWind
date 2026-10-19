using System.Linq.Expressions;
namespace NorthWind.Validation.Entities.Interfaces;
public interface IValidationRules<T,TProperty>
{
 IValidationRules<T,TProperty> NotEmpty(string message); IValidationRules<T,TProperty> NotNull(string message);
 IValidationRules<T,TProperty> Must(Func<TProperty,bool> predicate,string message); IValidationRules<T,TProperty> StopOnFirstFailure();
 IValidationRules<T,TProperty> GreaterThan<TValue>(TValue value,string message) where TValue:TProperty,IComparable<TValue>,IComparable;
 IValidationRules<T,TProperty> Equal(Expression<Func<T,TProperty>> expression,string message);
 IValidationRules<T,string> Length(int length,string message); IValidationRules<T,string> MaximumLength(int length,string message);
 IValidationRules<T,string> MinimumLength(int length,string message); IValidationRules<T,string> EmailAddress(string message);
}
