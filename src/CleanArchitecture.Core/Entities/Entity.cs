using CleanArchitecture.Core.Exceptions;

namespace CleanArchitecture.Core.Entities;

public abstract class Entity
{
    protected static void AssertRule(BusinessRule rule)
    {
        if (!rule.IsSatisfied()) throw new BusinessRuleValidationException(rule);
    }
}