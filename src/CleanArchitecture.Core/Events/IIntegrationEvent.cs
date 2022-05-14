namespace CleanArchitecture.Core.Events;

public interface IIntegrationEvent
{
}

// TODO: Make implementer implement IIntegrationEvent below but that it returns the concrete TIntegrationEvent impl
// TODO: Make implementer implement the EventBase class as well
public interface IIntegrationEvent<in TDomainEvent> : IIntegrationEvent where TDomainEvent : DomainEvent
{
}

public interface IIntegrationEvent<in TDomainEvent, out TIntegrationEvent> : IIntegrationEvent<TDomainEvent>
    where TDomainEvent : DomainEvent
{
    TIntegrationEvent From(TDomainEvent domainEvent);
}