using System.Threading.Tasks;
using CleanArchitecture.Core.Analyzers.NoPublicConstructorOnEntity;
using CleanArchitecture.Core.Entities;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using Verify =
    CleanArchitecture.Core.Analyzers.Tests.Verifiers.CSharpAnalyzerVerifier<
        CleanArchitecture.Core.Analyzers.NoPublicConstructorOnEntity.NoPublicConstructorOnEntityAnalyzer>;


namespace CleanArchitecture.Core.Analyzers.Tests;

public class NoPublicConstructorOnEntityTests : AnalyzerTestBase
{
    private const string EntityCode = $@"
public abstract class {nameof(Entity)} {{
}}
";

    private const string AggregateRootCode = $@"
{EntityCode}

public abstract class {nameof(AggregateRoot)} : {nameof(Entity)} {{
}}
";

    private static string GetEntitySource(string ctorType) => $@"
{EntityCode}

public class MyTestClass : {nameof(Entity)}
{{
    {ctorType} MyTestClass()
    {{
    }}
}}
";

    private static string GetAggregateRootSource() => $@"
{AggregateRootCode}

public class MyTestClass : {nameof(AggregateRoot)}
{{
    public MyTestClass()
    {{
    }}
}}
";

    [Fact]
    public async Task Public_constructor_has_diagnostic()
    {
        var expectedDiagnostic = DiagnosticResult
            .CompilerError(NoPublicConstructorOnEntityAnalyzer.DiagnosticId)
            .WithMessageFormat(NoPublicConstructorOnEntityAnalyzer.MessageFormat)
            .WithLocation(Location, 9, 5);


        await Verify.VerifyAnalyzerAsync(GetEntitySource("public"), expectedDiagnostic);
    }

    [Fact]
    public async Task Internal_constructor_has_no_diagnostic()
    {
        await Verify.VerifyAnalyzerAsync(GetEntitySource("internal"));
    }

    [Fact]
    public async Task Protected_constructor_has_no_diagnostic()
    {
        await Verify.VerifyAnalyzerAsync(GetEntitySource("protected"));
    }

    [Fact]
    public async Task Private_constructor_has_no_diagnostic()
    {
        await Verify.VerifyAnalyzerAsync(GetEntitySource("private"));
    }

    [Fact]
    public async Task AggregateRoot_has_no_diagnostic()
    {
        await Verify.VerifyAnalyzerAsync(GetAggregateRootSource());
    }
}