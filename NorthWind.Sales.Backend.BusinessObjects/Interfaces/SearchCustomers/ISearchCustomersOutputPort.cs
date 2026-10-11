using NorthWind.Sales.Entities.Dtos.SearchCustomers;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchCustomers;

public interface ISearchCustomersOutputPort
{
    IEnumerable<CustomerDto> Customers { get; }
    Task Handle(IEnumerable<CustomerDto> customers);
}
