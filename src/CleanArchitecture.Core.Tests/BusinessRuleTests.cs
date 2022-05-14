using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Core.Tests;

public class BusinessRuleTests
{
    [Fact]
    public void Satisfying_a_business_rule_does_not_throw_exception()
    {
        DummyEntity.TestRule(true);
    }

    [Fact]
    public void Not_satisfying_a_business_rule_throws_exception()
    {
        var act = () => DummyEntity.TestRule(false);
        act.Should().ThrowExactly<BusinessRuleValidationException>();
    }

    [Fact]
    public void Default_BusinessRuleValidationException_message_is_nameof_BusinessRule()
    {
        var act = () => DummyEntity.TestRule(false);

        act.Should().ThrowExactly<BusinessRuleValidationException>()
            .WithMessage(nameof(DummyBusinessRule));
    }

    [Fact]
    public void BusinessRuleValidationException_message_is_message_of_BusinessRule()
    {
        const string message = "message";
        var act = () => DummyEntity.TestRule(false, message);

        act.Should().ThrowExactly<BusinessRuleValidationException>()
            .WithMessage(message);
    }


    [Fact]
    public void BusinessRuleValidationException_ToString_Format()
    {
        // Arrange
        const string message = "message";
        var toStringResult = $"{typeof(DummyBusinessRule).FullName}: {message}";

        // Act 
        try
        {
            DummyEntity.TestRule(false, message);
        }
        catch (BusinessRuleValidationException exception)
        {
            //Assert
            exception.ToString().Should().Be(toStringResult);
        }
    }
}