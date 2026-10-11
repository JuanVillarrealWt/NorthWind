using System.Net.Http.Json;
using NorthWind.Sales.Entities.ValueObjects;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;
using NorthWind.Sales.Frontend.BusinessObjects.ViewModels;

namespace NorthWind.Sales.Frontend.WebApiGateways;

internal class SearchCustomersGateway(HttpClient client) : ISearchCustomersGateway
{
    public async Task<IEnumerable<CustomerItemViewModel>> SearchCustomers(string? filter)
    {
        var url = string.IsNullOrWhiteSpace(filter)
            ? Endpoints.SearchCustomers
            : $"{Endpoints.SearchCustomers}?filter={Uri.EscapeDataString(filter)}";

        var response = await client.GetFromJsonAsync<IEnumerable<CustomerItemViewModel>>(url);
        return response ?? [];
    }
}
