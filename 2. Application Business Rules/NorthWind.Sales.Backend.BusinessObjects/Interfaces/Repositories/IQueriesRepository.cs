using NorthWind.Sales.Backend.BusinessObjects.ValueObjects;
using NorthWind.Sales.Entities.Dtos.SearchCustomers;
using NorthWind.Sales.Entities.Dtos.SearchOrders;
using NorthWind.Sales.Entities.Dtos.SearchProducts;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
public interface IQueriesRepository
{
    Task<decimal?> GetCustomerCurrentBalance(string customerId);
    Task<IEnumerable<ProductUnitsInStock>> GetProductsUnitsInStock(IEnumerable<int> productIds);
    Task<IEnumerable<CustomerDto>> SearchCustomers(string? filter);
    Task<IEnumerable<ProductDto>> SearchProducts(string? filter);
    Task<IEnumerable<OrderHeaderDto>> SearchOrders(string? filter);
    Task<OrderInvoiceDto?> GetOrderById(int orderId);
}


