namespace NorthWind.HttpDelegatingHandlers;
public sealed class ExceptionDelegatingHandler : DelegatingHandler
{
 protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
 {
  var response=await base.SendAsync(request,cancellationToken);
  if(response.IsSuccessStatusCode) return response;
  var body=await response.Content.ReadAsStringAsync(cancellationToken);
  var message=body;
  string? source=null;
  IEnumerable<ValidationError>? errors=null;
  var isProblem=false;
  if(response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
   message=$"{(int)response.StatusCode} {response.ReasonPhrase}";
  else if(string.Equals(response.Content.Headers.ContentType?.MediaType,"application/problem+json",StringComparison.OrdinalIgnoreCase))
  {
   try
   {
    using var document=JsonDocument.Parse(body);
    var root=document.RootElement;
    if(TryGetProperty(root,"instance",out var instance) && instance.GetString() is { } instanceText && instanceText.StartsWith("ProblemDetails/",StringComparison.OrdinalIgnoreCase))
    {
     source=instanceText;
     var title=TryGetProperty(root,"title",out var titleValue) ? titleValue.ToString() : string.Empty;
     var detail=TryGetProperty(root,"detail",out var detailValue) ? detailValue.ToString() : string.Empty;
     message=string.Join(" ",new[]{title,detail}.Where(value=>!string.IsNullOrWhiteSpace(value)));
     if(TryGetProperty(root,"errors",out var errorsValue)) errors=errorsValue.Deserialize<List<ValidationError>>(new JsonSerializerOptions{PropertyNameCaseInsensitive=true});
     isProblem=true;
    }
   }
   catch(JsonException) { }
  }
  if(!isProblem && string.IsNullOrWhiteSpace(message)) message=$"{(int)response.StatusCode} {response.ReasonPhrase}";
  var exception=new HttpRequestException(message,null,response.StatusCode) { Source=source };
  if(errors is not null) exception.Data["Errors"]=errors;
  response.Dispose();
  throw exception;
 }
 private static bool TryGetProperty(JsonElement element,string name,out JsonElement value)
 {
  if(element.ValueKind==JsonValueKind.Object)
   foreach(var property in element.EnumerateObject())
    if(string.Equals(property.Name,name,StringComparison.OrdinalIgnoreCase)){value=property.Value;return true;}
  value=default;return false;
 }
}
