using System.Net.Http.Json;
using NorthWind.Sales.Entities.Dtos.SearchOrders;
using NorthWind.Sales.Entities.ValueObjects;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

namespace NorthWind.Sales.Frontend.WebApiGateways;

internal class SearchOrdersGateway(HttpClient client) : ISearchOrdersGateway
{
    public async Task<IEnumerable<OrderHeaderDto>> SearchOrders(string? filter)
    {
        var url = string.IsNullOrWhiteSpace(filter)
            ? Endpoints.SearchOrders
            : $"{Endpoints.SearchOrders}?filter={Uri.EscapeDataString(filter)}";

        var response = await client.GetFromJsonAsync<IEnumerable<OrderHeaderDto>>(url);
        return response ?? [];
    }

    public async Task<OrderInvoiceDto?> GetOrderById(int orderId)
    {
        var url = $"{Endpoints.GetOrderById}?id={orderId}";
        return await client.GetFromJsonAsync<OrderInvoiceDto>(url);
    }
}
