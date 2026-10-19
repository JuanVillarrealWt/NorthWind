namespace NorthWind.Sales.Backend.Repositories.Repositories;

internal class CommandsRepository(
    INorthWindSalesCommandsDataContext context)
    : ICommandsRepository
{
    public async Task CreateOrder(OrderAggregate order)
    {
        await context.AddOrderAsync(order);

        await context.AddOrderDetailsAsync(
            order.OrderDetails
                .Select(d => new Entities.OrderDetail
                {
                    Order = order,
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                })
                .ToArray());

        // Requerimiento 7: Disminución AUTOMÁTICA del inventario en la tabla correspondiente (Products)
        var stockDeductions = order.OrderDetails.Select(d => (d.ProductId, (int)d.Quantity));
        await context.DeductProductStockAsync(stockDeductions);
    }

    public async Task CreateCustomer(NorthWind.Sales.Entities.Dtos.SearchCustomers.CustomerDto customer) =>
        await context.CreateCustomerAsync(new Entities.Customer { Id = customer.Id, Name = customer.Name, CurrentBalance = customer.CurrentBalance });

    public async Task UpdateCustomer(NorthWind.Sales.Entities.Dtos.SearchCustomers.CustomerDto customer) =>
        await context.UpdateCustomerAsync(new Entities.Customer { Id = customer.Id, Name = customer.Name, CurrentBalance = customer.CurrentBalance });

    public async Task DeleteCustomer(string customerId) =>
        await context.DeleteCustomerAsync(customerId);

    public async Task CreateProduct(NorthWind.Sales.Entities.Dtos.SearchProducts.ProductDto product) =>
        await context.CreateProductAsync(new Entities.Product { Name = product.Name, UnitPrice = product.UnitPrice, UnitsInStock = (short)product.UnitsInStock });

    public async Task UpdateProduct(NorthWind.Sales.Entities.Dtos.SearchProducts.ProductDto product) =>
        await context.UpdateProductAsync(new Entities.Product { Id = product.Id, Name = product.Name, UnitPrice = product.UnitPrice, UnitsInStock = (short)product.UnitsInStock });

    public async Task DeleteProduct(int productId) =>
        await context.DeleteProductAsync(productId);

    public async Task SaveChanges() =>
        await context.SaveChangesAsync();
}
