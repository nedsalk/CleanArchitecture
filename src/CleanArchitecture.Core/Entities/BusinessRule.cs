namespace CleanArchitecture.Core.Entities;

public abstract class BusinessRule
{
    public abstract bool IsSatisfied();

    public virtual string? Message => null;
}