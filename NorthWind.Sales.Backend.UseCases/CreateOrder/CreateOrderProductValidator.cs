namespace NorthWind.Sales.Backend.UseCases.CreateOrder;
internal sealed class CreateOrderProductValidator(IQueriesRepository repository) : IModelValidator<CreateOrderDto>
{
 private List<ValidationError> currentErrors=[];
 public IEnumerable<ValidationError> Errors=>currentErrors;
 public ValidationConstraint Constraint=>ValidationConstraint.ValidateIfThereAreNoPreviousErrors;
 public async Task<bool> Validate(CreateOrderDto model)
 {
  currentErrors=[];
  var lines=model.OrderDetails.ToList();

  // Rule 3: Un producto debe aparecer una sola vez en la orden
  var duplicates = lines.GroupBy(d => d.ProductId).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
  foreach (var dupId in duplicates)
  {
      var index = lines.FindLastIndex(d => d.ProductId == dupId);
      currentErrors.Add(new($"OrderDetails[{index}].ProductId", string.Format(Resources.CreateOrderMessages.DuplicateProductInOrderError, dupId)));
  }

  var quantities=lines.GroupBy(d=>d.ProductId).Select(g=>new { ProductId=g.Key,Required=g.Sum(d=>(int)d.Quantity) }).ToList();
  var stock=await repository.GetProductsUnitsInStock(quantities.Select(q=>q.ProductId));
  foreach(var required in quantities)
  {
   var available=stock.FirstOrDefault(p=>p.ProductId==required.ProductId);
   var index=lines.FindIndex(d=>d.ProductId==required.ProductId);
   if(available is null)
    currentErrors.Add(new($"OrderDetails[{index}].ProductId",string.Format(Resources.CreateOrderMessages.ProductIdNotFoundErrorTemplate,required.ProductId)));
   else if(available.UnitsInStock<required.Required)
    currentErrors.Add(new($"OrderDetails[{index}].Quantity",string.Format(Resources.CreateOrderMessages.UnitsInStockLessThanQuantityErrorTemplate,required.Required,available.UnitsInStock,required.ProductId)));
  }
  return currentErrors.Count==0;
 }
}

