using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchCustomers;

namespace NorthWind.Sales.Backend.UseCases.SearchCustomers;

internal sealed class SearchCustomersInteractor(
    IQueriesRepository repository,
    ISearchCustomersOutputPort outputPort) : ISearchCustomersInputPort
{
    public async Task SearchCustomers(string? filter)
    {
        var customers = await repository.SearchCustomers(filter);
        await outputPort.Handle(customers);
    }
}
