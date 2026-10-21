namespace NorthWind.Sales.Backend.BusinessObjects.Aggregates;

public class OrderAggregate : Order
{
    readonly List<OrderDetail> OrderDetailsField = [];

    public IReadOnlyCollection<OrderDetail> OrderDetails =>
        OrderDetailsField;

    public void AddDetail(int productId, decimal unitPrice, short quantity)
    {
        var existingOrderDetail = OrderDetailsField
            .FirstOrDefault(o => o.ProductId == productId);

        if (existingOrderDetail != default)
        {
            quantity += existingOrderDetail.Quantity;
            OrderDetailsField.Remove(existingOrderDetail);
        }

        OrderDetailsField.Add(
            new OrderDetail(productId, unitPrice, quantity));
    }

    public static OrderAggregate From(CreateOrderDto orderDto)
    {
        OrderAggregate orderAggregate = new OrderAggregate
        {
            CustomerId = orderDto.CustomerId?.Trim() ?? string.Empty,
            ShipAddress = System.Text.RegularExpressions.Regex.Replace(orderDto.ShipAddress?.Trim() ?? string.Empty, @"\s{2,}", " "),
            ShipCity = System.Text.RegularExpressions.Regex.Replace(orderDto.ShipCity?.Trim() ?? string.Empty, @"\s{2,}", " "),
            ShipCountry = System.Text.RegularExpressions.Regex.Replace(orderDto.ShipCountry?.Trim() ?? string.Empty, @"\s{2,}", " "),
            ShipPostalCode = orderDto.ShipPostalCode?.Trim() ?? string.Empty
        };

        foreach (var item in orderDto.OrderDetails)
        {
            orderAggregate.AddDetail(
                item.ProductId,
                item.UnitPrice,
                item.Quantity);
        }

        return orderAggregate;
    }
}
