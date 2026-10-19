using NorthWind.Sales.Frontend.BusinessObjects.ViewModels;

namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface ICustomerCrudGateway
{
    Task CreateCustomer(CustomerItemViewModel customer);
    Task UpdateCustomer(CustomerItemViewModel customer);
    Task DeleteCustomer(string id);
}
