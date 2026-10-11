namespace NorthWind.Events.Entities.Interfaces;
public interface IDomainEventHub<TEvent> where TEvent:IDomainEvent
{
 Task Raise(TEvent eventTypeInstance);
}
