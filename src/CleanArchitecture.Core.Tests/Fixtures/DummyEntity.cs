using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Core.Tests.Fixtures;

public class DummyEntity : Entity
{
    public static void TestRule(bool isSatisfied, string? message = null)
    {
        AssertRule(new DummyBusinessRule(isSatisfied, message));
    }
}