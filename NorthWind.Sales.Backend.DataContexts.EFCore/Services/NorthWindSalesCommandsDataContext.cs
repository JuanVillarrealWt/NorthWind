namespace NorthWind.Sales.Backend.DataContexts.EFCore.Services;

internal class NorthWindSalesCommandsDataContext(IOptions<DBOptions> dbOptions)
    : NorthWindSalesContext(dbOptions), INorthWindSalesCommandsDataContext
{
    public async Task AddOrderAsync(Order order) => await AddAsync(order);

    public async Task AddOrderDetailsAsync(
        IEnumerable<NorthWind.Sales.Backend.Repositories.Entities.OrderDetail> orderDetails) =>
        await AddRangeAsync(orderDetails);

    public async Task DeductProductStockAsync(IEnumerable<(int ProductId, int Quantity)> items)
    {
        foreach (var item in items)
        {
            var product = await Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
            if (product != null)
            {
                product.UnitsInStock = (short)Math.Max(0, product.UnitsInStock - item.Quantity);
            }
        }
    }

    public async Task CreateCustomerAsync(Customer customer) => await Customers.AddAsync(customer);

    public async Task UpdateCustomerAsync(Customer customer)
    {
        var existing = await Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);
        if (existing != null)
        {
            existing.Name = customer.Name;
            existing.CurrentBalance = customer.CurrentBalance;
        }
    }

    public async Task DeleteCustomerAsync(string customerId)
    {
        var existing = await Customers.FirstOrDefaultAsync(c => c.Id == customerId);
        if (existing != null)
        {
            Customers.Remove(existing);
        }
    }

    public async Task CreateProductAsync(Product product) => await Products.AddAsync(product);

    public async Task UpdateProductAsync(Product product)
    {
        var existing = await Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        if (existing != null)
        {
            existing.Name = product.Name;
            existing.UnitPrice = product.UnitPrice;
            existing.UnitsInStock = product.UnitsInStock;
        }
    }

    public async Task DeleteProductAsync(int productId)
    {
        var existing = await Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (existing != null)
        {
            Products.Remove(existing);
        }
    }

    public async Task SaveChangesAsync() => await GuardDBContext.AgainstSaveChangesErrorAsync(this);
}
