
namespace NorthWind.Sales.Entities.ValueObjects;

public class Endpoints
{
    public const string CreateOrder = $"/{nameof(CreateOrder)}";
    public const string SearchCustomers = $"/{nameof(SearchCustomers)}";
    public const string SearchProducts = $"/{nameof(SearchProducts)}";
    public const string GetOrderById = $"/{nameof(GetOrderById)}";
    public const string SearchOrders = $"/{nameof(SearchOrders)}";

    public const string CreateCustomer = $"/{nameof(CreateCustomer)}";
    public const string UpdateCustomer = $"/{nameof(UpdateCustomer)}";
    public const string DeleteCustomer = $"/{nameof(DeleteCustomer)}";

    public const string CreateProduct = $"/{nameof(CreateProduct)}";
    public const string UpdateProduct = $"/{nameof(UpdateProduct)}";
    public const string DeleteProduct = $"/{nameof(DeleteProduct)}";
}

