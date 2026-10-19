namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.GetOrderById;

public interface IGetOrderByIdInputPort
{
    Task GetOrderById(int orderId);
}
