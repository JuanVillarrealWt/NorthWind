using NorthWind.Sales.Entities.Dtos.SearchCustomers;
using NorthWind.Sales.Entities.Dtos.SearchOrders;
using NorthWind.Sales.Entities.Dtos.SearchProducts;

namespace NorthWind.Sales.Backend.Repositories.Repositories;

internal sealed class QueriesRepository(INorthWindSalesQueriesDataContext context) : IQueriesRepository
{
    public async Task<decimal?> GetCustomerCurrentBalance(string customerId)
    {
        // Keep the projection nullable so FirstOrDefaultAsync can distinguish
        // a missing customer from an existing customer whose balance is zero.
        var result = await context.FirstOrDefaultAsync(
            context.Customers
                .Where(c => c.Id == customerId)
                .Select(c => (decimal?)c.CurrentBalance));
        return result;
    }

    public Task<IEnumerable<ProductUnitsInStock>> GetProductsUnitsInStock(IEnumerable<int> productIds) =>
        context.ToListAsync(context.Products.Where(p => productIds.Contains(p.Id)).Select(p => new ProductUnitsInStock(p.Id, p.UnitsInStock)));

    public Task<IEnumerable<CustomerDto>> SearchCustomers(string? filter)
    {
        var query = context.Customers.AsQueryable();
        if (!string.IsNullOrWhiteSpace(filter))
        {
            var term = filter.Trim().ToLower();
            query = query.Where(c => c.Id.ToLower().Contains(term) || c.Name.ToLower().Contains(term));
        }

        return context.ToListAsync(query.Select(c => new CustomerDto
        {
            Id = c.Id,
            Name = c.Name,
            CurrentBalance = c.CurrentBalance
        }));
    }

    public Task<IEnumerable<ProductDto>> SearchProducts(string? filter)
    {
        // Requerimiento 8: Mostrar SOLO productos con stock mayor a cero (UnitsInStock > 0)
        var query = context.Products.Where(p => p.UnitsInStock > 0);
        if (!string.IsNullOrWhiteSpace(filter))
        {
            var term = filter.Trim().ToLower();
            query = query.Where(p => p.Id.ToString().Contains(term) || p.Name.ToLower().Contains(term));
        }

        return context.ToListAsync(query.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            UnitPrice = p.UnitPrice,
            UnitsInStock = p.UnitsInStock
        }));
    }


    public async Task<IEnumerable<OrderHeaderDto>> SearchOrders(string? filter)
    {
        var orders = context.Orders.AsQueryable();
        if (!string.IsNullOrWhiteSpace(filter))
        {
            var term = filter.Trim().ToLower();
            orders = orders.Where(o => o.Id.ToString().Contains(term) || o.CustomerId.ToLower().Contains(term));
        }

        var resultList = await context.ToListAsync(orders);
        var headers = new List<OrderHeaderDto>();
        foreach (var order in resultList)
        {
            var details = await context.ToListAsync(context.OrderDetails.Where(d => d.OrderId == order.Id));
            headers.Add(new OrderHeaderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = string.IsNullOrEmpty(order.CustomerName) ? order.CustomerId : order.CustomerName,
                SalespersonName = order.SalespersonName,
                OrderDate = order.OrderDate,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipCountry = order.ShipCountry,
                ShipPostalCode = order.ShipPostalCode,
                Total = details.Sum(d => d.UnitPrice * d.Quantity)
            });
        }

        return headers.OrderByDescending(h => h.Id);
    }

    public async Task<OrderInvoiceDto?> GetOrderById(int orderId)
    {
        var order = await context.FirstOrDefaultAsync(context.Orders.Where(o => o.Id == orderId));
        if (order == null) return null;

        var detailsEntities = await context.ToListAsync(context.OrderDetails.Where(d => d.OrderId == orderId));

        var details = detailsEntities.Select(d => new OrderDetailItemDto
        {
            ProductId = d.ProductId,
            ProductName = string.IsNullOrEmpty(d.ProductName) ? $"Producto #{d.ProductId}" : d.ProductName,
            UnitPrice = d.UnitPrice,
            Quantity = d.Quantity
        }).ToList();

        var header = new OrderHeaderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CustomerName = string.IsNullOrEmpty(order.CustomerName) ? order.CustomerId : order.CustomerName,
            SalespersonName = order.SalespersonName,
            OrderDate = order.OrderDate,
            ShipAddress = order.ShipAddress,
            ShipCity = order.ShipCity,
            ShipCountry = order.ShipCountry,
            ShipPostalCode = order.ShipPostalCode,
            Total = details.Sum(d => d.Subtotal)
        };

        return new OrderInvoiceDto
        {
            Header = header,
            Details = details
        };
    }
}


