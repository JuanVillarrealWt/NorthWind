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
            CustomerId = orderDto.CustomerId,
            ShipAddress = orderDto.ShipAddress,
            ShipCity = orderDto.ShipCity,
            ShipCountry = orderDto.ShipCountry,
            ShipPostalCode = orderDto.ShipPostalCode
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
