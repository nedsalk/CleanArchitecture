using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Core.Tests.Fixtures;

public class DummyBusinessRule : BusinessRule
{
    private readonly bool _isSatisfied;

    public DummyBusinessRule(bool isSatisfied, string? message)
    {
        _isSatisfied = isSatisfied;
        Message = message;
    }

    public override bool IsSatisfied()
    {
        return _isSatisfied;
    }

    public override string? Message { get; }
}