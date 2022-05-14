
using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Core.Exceptions;

#pragma warning disable CA1032 // Implement standard exception constructors
public class BusinessRuleValidationException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
{
    public BusinessRuleValidationException(BusinessRule brokenRule)
        : base(brokenRule.Message ?? brokenRule.GetType().Name)
    {
        BrokenRule = brokenRule;
    }

    private BusinessRule BrokenRule { get; }

    public override string ToString()
    {
        return $"{BrokenRule.GetType().FullName}: {Message}";
    }
}