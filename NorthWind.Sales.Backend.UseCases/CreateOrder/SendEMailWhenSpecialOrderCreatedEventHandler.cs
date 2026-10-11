using NorthWind.Entities.Interfaces;
using NorthWind.Sales.Backend.UseCases.Resources;

namespace NorthWind.Sales.Backend.UseCases.CreateOrder;

internal sealed class SendEMailWhenSpecialOrderCreatedEventHandler(IMailService mailService)
    : IDomainEventHandler<SpecialOrderCreatedEvent>
{
    public Task Handle(SpecialOrderCreatedEvent createdOrder) =>
        mailService.SendMailToAdministrator(
            string.Format(CreateOrderMessages.SendEmailSubject, createdOrder.OrderId),
            string.Format(CreateOrderMessages.SendEmailBodyTemplate, createdOrder.OrderId, createdOrder.ProductsCount));
}

