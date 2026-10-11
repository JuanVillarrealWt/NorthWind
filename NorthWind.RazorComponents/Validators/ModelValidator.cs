namespace NorthWind.RazorComponents.Validators;
public sealed class ModelValidator<T> : ComponentBase, IDisposable
{
 [CascadingParameter] private EditContext? EditContext { get; set; }
 [Parameter, EditorRequired] public IModelValidatorHub<T> Validator { get; set; } = default!;
 private ValidationMessageStore? messageStore;
 private EditContext? subscribedContext;

 protected override void OnParametersSet()
 {
  if(EditContext is null) throw new InvalidOperationException("ModelValidator must be nested inside an EditForm.");
  if(!ReferenceEquals(EditContext,subscribedContext))
  {
   Detach(); subscribedContext=EditContext; messageStore=new ValidationMessageStore(EditContext);
   EditContext.OnValidationRequested+=ValidationRequested; EditContext.OnFieldChanged+=FieldChanged;
  }
 }
 private async void ValidationRequested(object? sender,ValidationRequestedEventArgs args)
 {
  if(subscribedContext is null || messageStore is null) return;
  if(await Validator.Validate((T)subscribedContext.Model)) messageStore.Clear(); else AddErrors(Validator.Errors);
  subscribedContext.NotifyValidationStateChanged();
 }
 private async void FieldChanged(object? sender,FieldChangedEventArgs args)
 {
  if(subscribedContext is null || messageStore is null) return;
  messageStore.Clear(args.FieldIdentifier);
  if(!await Validator.Validate((T)subscribedContext.Model))
   foreach(var error in Validator.Errors)
   {
    var field=GetFieldIdentifier(subscribedContext.Model,error.PropertyName);
    if(field.Equals(args.FieldIdentifier)) messageStore.Add(field,error.Message);
   }
  subscribedContext.NotifyValidationStateChanged();
 }
 public void AddErrors(IEnumerable<ValidationError> errors)
 {
  if(subscribedContext is null || messageStore is null) return;
  messageStore.Clear();
  foreach(var error in errors) messageStore.Add(GetFieldIdentifier(subscribedContext.Model,error.PropertyName),error.Message);
  subscribedContext.NotifyValidationStateChanged();
 }
 private static FieldIdentifier GetFieldIdentifier(object model,string path)
 {
  object current=model; var parts=System.Text.RegularExpressions.Regex.Matches(path,@"[^.\[\]]+").Select(m=>m.Value).ToArray();
  if(parts.Length==0) return new FieldIdentifier(model,path);
  for(var i=0;i<parts.Length-1;i++)
  {
   var type=current.GetType(); var property=type.GetProperty(parts[i]);
   if(property is not null) { current=property.GetValue(current) ?? current; continue; }
   if(int.TryParse(parts[i],out var index) && current is System.Collections.IList list && index>=0 && index<list.Count) current=list[index]!;
  }
  return new FieldIdentifier(current,parts[^1]);
 }
 private void Detach()
 {
  if(subscribedContext is not null) { subscribedContext.OnValidationRequested-=ValidationRequested; subscribedContext.OnFieldChanged-=FieldChanged; }
 }
 public void Dispose()=>Detach();
}
