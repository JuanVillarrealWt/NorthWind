namespace NorthWind.Sales.Backend.DataContexts.EFCore.Services;
internal sealed class NorthWindSalesQueriesDataContext : NorthWindSalesContext, INorthWindSalesQueriesDataContext
{
    public NorthWindSalesQueriesDataContext(IOptions<DBOptions> options) : base(options) { ChangeTracker.QueryTrackingBehavior=QueryTrackingBehavior.NoTracking; }
    IQueryable<Customer> INorthWindSalesQueriesDataContext.Customers => Customers;
    IQueryable<Product> INorthWindSalesQueriesDataContext.Products => Products;
    IQueryable<Order> INorthWindSalesQueriesDataContext.Orders => Orders;
    IQueryable<NorthWind.Sales.Backend.Repositories.Entities.OrderDetail> INorthWindSalesQueriesDataContext.OrderDetails => OrderDetails;
    public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> queryable) => EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(queryable);
    public async Task<IEnumerable<T>> ToListAsync<T>(IQueryable<T> queryable) => await EntityFrameworkQueryableExtensions.ToListAsync(queryable);
}

