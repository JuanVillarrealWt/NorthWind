using System.Net.Http.Json;
using NorthWind.Sales.Entities.ValueObjects;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;
using NorthWind.Sales.Frontend.BusinessObjects.ViewModels;

namespace NorthWind.Sales.Frontend.WebApiGateways;

internal class SearchProductsGateway(HttpClient client) : ISearchProductsGateway
{
    public async Task<IEnumerable<ProductItemViewModel>> SearchProducts(string? filter)
    {
        var url = string.IsNullOrWhiteSpace(filter)
            ? Endpoints.SearchProducts
            : $"{Endpoints.SearchProducts}?filter={Uri.EscapeDataString(filter)}";

        var response = await client.GetFromJsonAsync<IEnumerable<ProductItemViewModel>>(url);
        return response ?? [];
    }
}
