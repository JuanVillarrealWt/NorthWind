namespace NorthWind.Events.Entities.Interfaces;
public interface IDomainEventHandler<TEvent> where TEvent:IDomainEvent
{
 Task Handle(TEvent eventTypeInstance);
}
