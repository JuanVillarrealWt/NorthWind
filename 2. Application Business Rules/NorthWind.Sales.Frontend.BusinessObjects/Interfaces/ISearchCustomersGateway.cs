using NorthWind.Sales.Frontend.BusinessObjects.ViewModels;

namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface ISearchCustomersGateway
{
    Task<IEnumerable<CustomerItemViewModel>> SearchCustomers(string? filter);
}
