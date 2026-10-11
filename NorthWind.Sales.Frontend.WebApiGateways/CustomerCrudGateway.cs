using System.Net.Http.Json;
using NorthWind.Sales.Entities.ValueObjects;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;
using NorthWind.Sales.Frontend.BusinessObjects.ViewModels;

namespace NorthWind.Sales.Frontend.WebApiGateways;

internal class CustomerCrudGateway(HttpClient client) : ICustomerCrudGateway
{
    public async Task CreateCustomer(CustomerItemViewModel customer)
    {
        await client.PostAsJsonAsync(Endpoints.CreateCustomer, customer);
    }

    public async Task UpdateCustomer(CustomerItemViewModel customer)
    {
        await client.PutAsJsonAsync(Endpoints.UpdateCustomer, customer);
    }

    public async Task DeleteCustomer(string id)
    {
        await client.DeleteAsync($"{Endpoints.DeleteCustomer}/{Uri.EscapeDataString(id)}");
    }
}
