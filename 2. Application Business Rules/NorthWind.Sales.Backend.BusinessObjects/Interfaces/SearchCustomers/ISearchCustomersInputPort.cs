namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchCustomers;

public interface ISearchCustomersInputPort
{
    Task SearchCustomers(string? filter);
}
