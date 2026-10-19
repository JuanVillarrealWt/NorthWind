namespace NorthWind.Sales.Backend.Repositories.Interfaces;

public interface INorthWindSalesCommandsDataContext
{
    Task AddOrderAsync(Order order);

    Task AddOrderDetailsAsync(
        IEnumerable<Entities.OrderDetail> orderDetails);

    Task DeductProductStockAsync(IEnumerable<(int ProductId, int Quantity)> items);

    Task CreateCustomerAsync(Customer customer);
    Task UpdateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(string customerId);

    Task CreateProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int productId);

    Task SaveChangesAsync();
}
