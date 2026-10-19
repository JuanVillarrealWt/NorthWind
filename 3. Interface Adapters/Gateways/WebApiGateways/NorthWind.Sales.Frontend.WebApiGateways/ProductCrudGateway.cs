using System.Net.Http.Json;
using NorthWind.Sales.Entities.ValueObjects;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;
using NorthWind.Sales.Frontend.BusinessObjects.ViewModels;

namespace NorthWind.Sales.Frontend.WebApiGateways;

internal class ProductCrudGateway(HttpClient client) : IProductCrudGateway
{
    public async Task CreateProduct(ProductItemViewModel product)
    {
        await client.PostAsJsonAsync(Endpoints.CreateProduct, product);
    }

    public async Task UpdateProduct(ProductItemViewModel product)
    {
        await client.PutAsJsonAsync(Endpoints.UpdateProduct, product);
    }

    public async Task DeleteProduct(int id)
    {
        await client.DeleteAsync($"{Endpoints.DeleteProduct}/{id}");
    }
}
