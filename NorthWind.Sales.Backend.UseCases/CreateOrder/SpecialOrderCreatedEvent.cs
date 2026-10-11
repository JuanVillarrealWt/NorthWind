namespace NorthWind.Sales.Backend.UseCases.CreateOrder;

public sealed record SpecialOrderCreatedEvent(int OrderId, int ProductsCount) : IDomainEvent;
