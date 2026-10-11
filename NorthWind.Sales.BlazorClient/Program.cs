using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NorthWind.Sales.BlazorClient;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddNorthWindSalesServices(client =>
{
    var webApiAddress = builder.Configuration["WebApiAddress"]
        ?? throw new InvalidOperationException("WebApiAddress is missing from appsettings.json.");
    client.BaseAddress = new Uri(webApiAddress);
}, membershipHttpClient =>
{
    var webApiAddress = builder.Configuration["WebApiAddress"]
        ?? throw new InvalidOperationException("WebApiAddress is missing from appsettings.json.");
    membershipHttpClient.BaseAddress = new Uri(webApiAddress);
}, httpClientBuilder => httpClientBuilder.AddMembershipBearerTokenHandler());

await builder.Build().RunAsync();
