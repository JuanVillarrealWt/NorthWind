namespace NorthWind.Sales.Backend.UseCases.CreateOrder;
internal sealed class CreateOrderCustomerValidator(IQueriesRepository repository) : IModelValidator<CreateOrderDto>
{
 private List<ValidationError> currentErrors=[];
 public IEnumerable<ValidationError> Errors=>currentErrors;
 public ValidationConstraint Constraint=>ValidationConstraint.ValidateIfThereAreNoPreviousErrors;
 public async Task<bool> Validate(CreateOrderDto model)
 {
  currentErrors=[];
  var balance=await repository.GetCustomerCurrentBalance(model.CustomerId);
  if(balance is null) currentErrors.Add(new(nameof(model.CustomerId),Resources.CreateOrderMessages.CustomerIdNotFoundError));
  else if(balance>0) currentErrors.Add(new(nameof(model.CustomerId),string.Format(Resources.CreateOrderMessages.CustomerWithBalanceErrorTemplate,model.CustomerId,balance)));
  return currentErrors.Count==0;
 }
}
