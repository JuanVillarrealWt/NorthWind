namespace NorthWind.Events.Entities.Services;
internal sealed class DomainEventHub<TEvent>(IEnumerable<IDomainEventHandler<TEvent>> handlers) : IDomainEventHub<TEvent> where TEvent:IDomainEvent
{
 public async Task Raise(TEvent eventTypeInstance)
 {
  foreach(var handler in handlers) await handler.Handle(eventTypeInstance);
 }
}
