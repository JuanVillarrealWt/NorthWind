using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchCustomers;
using NorthWind.Sales.Entities.Dtos.SearchCustomers;

namespace NorthWind.Sales.Backend.Presenters.SearchCustomers;

internal sealed class SearchCustomersPresenter : ISearchCustomersOutputPort
{
    public IEnumerable<CustomerDto> Customers { get; private set; } = [];

    public Task Handle(IEnumerable<CustomerDto> customers)
    {
        Customers = customers;
        return Task.CompletedTask;
    }
}
