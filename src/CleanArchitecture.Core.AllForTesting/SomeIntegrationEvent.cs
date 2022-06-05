using CleanArchitecture.Core.Events;

namespace CleanArchitecture.Core.AllForTesting;

public partial class SomeIntegrationEvent
{
    public SomeIntegrationEvent From(SomeDomainEvent domainEvent)
    {
        throw new NotImplementedException();
    }
}