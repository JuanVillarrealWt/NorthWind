namespace NorthWind.Sales.Backend.Repositories.Interfaces;
public interface INorthWindSalesQueriesDataContext
{
    IQueryable<Customer> Customers { get; }
    IQueryable<Product> Products { get; }
    IQueryable<Order> Orders { get; }
    IQueryable<NorthWind.Sales.Backend.Repositories.Entities.OrderDetail> OrderDetails { get; }
    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> queryable);
    Task<IEnumerable<T>> ToListAsync<T>(IQueryable<T> queryable);
}

