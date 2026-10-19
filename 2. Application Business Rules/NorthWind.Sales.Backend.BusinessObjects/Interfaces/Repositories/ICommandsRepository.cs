using NorthWind.Sales.Entities.Dtos.SearchCustomers;
using NorthWind.Sales.Entities.Dtos.SearchProducts;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;

public interface ICommandsRepository : IUnitOfWork
{
    Task CreateOrder(OrderAggregate order);

    Task CreateCustomer(CustomerDto customer);
    Task UpdateCustomer(CustomerDto customer);
    Task DeleteCustomer(string customerId);

    Task CreateProduct(ProductDto product);
    Task UpdateProduct(ProductDto product);
    Task DeleteProduct(int productId);
}
