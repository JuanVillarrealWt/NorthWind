namespace NorthWind.Sales.Backend.UseCases.CreateOrder;

using NorthWind.Sales.Backend.BusinessObjects.Specifications;
using NorthWind.Sales.Backend.UseCases.Resources;
using Microsoft.Extensions.Logging;

internal class CreateOrderInteractor(
    ICreateOrderOutputPort outputPort,
    ICommandsRepository repository,
    IModelValidatorHub<CreateOrderDto> modelValidatorHub,
    IDomainEventHub<SpecialOrderCreatedEvent> domainEventHub,
    IDomainLogger domainLogger,
    IDomainTransaction domainTransaction,
    IUserService userService,
    ILogger<CreateOrderInteractor> logger) : ICreateOrderInputPort
{
    public async Task Handle(CreateOrderDto orderDto)
    {
        GuardUser.AgainstUnauthenticated(userService);
        await GuardModel.AgainstNotValid(modelValidatorHub, orderDto);
        await domainLogger.LogInformation(new DomainLog(
            CreateOrderMessages.StartingPurchaseOrderCreation, userService.UserName));
        OrderAggregate order = OrderAggregate.From(orderDto);
        order.SalespersonName = !string.IsNullOrWhiteSpace(userService.FullName)
            ? userService.FullName
            : !string.IsNullOrWhiteSpace(userService.UserName)
                ? userService.UserName
                : "Vendedor Autorizado";

        try
        {
            domainTransaction.BeginTransaction();
            await repository.CreateOrder(order);
            await repository.SaveChanges();
            await domainLogger.LogInformation(new DomainLog(string.Format(
                CreateOrderMessages.PurchaseOrderCreatedTemplate, order.Id), userService.UserName));
            await outputPort.Handle(order);
            domainTransaction.CommitTransaction();
        }
        catch
        {
            domainTransaction.RollbackTransaction();
            await domainLogger.LogInformation(new DomainLog(string.Format(
                CreateOrderMessages.OrderCreationCancelledTemplate, order.Id), userService.UserName));
            throw;
        }

        if (new SpecialOrderSpecification().IsSatisfiedBy(order))
        {
            try
            {
                await domainEventHub.Raise(new SpecialOrderCreatedEvent(order.Id, order.OrderDetails.Count));
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, "Order {OrderId} was saved, but its special-order email notification failed.", order.Id);
            }
        }
    }
}
