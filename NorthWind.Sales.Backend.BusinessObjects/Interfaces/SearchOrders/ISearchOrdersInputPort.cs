namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchOrders;

public interface ISearchOrdersInputPort
{
    Task SearchOrders(string? filter);
}
