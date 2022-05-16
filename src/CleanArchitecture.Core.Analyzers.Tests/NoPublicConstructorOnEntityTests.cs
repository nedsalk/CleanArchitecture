using System.Threading.Tasks;
using CleanArchitecture.Core.Analyzers.NoPublicConstructorOnEntity;
using CleanArchitecture.Core.Entities;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using Verify =
    CleanArchitecture.Core.Analyzers.Tests.Verifiers.CsAnalyzerVerifier<
        CleanArchitecture.Core.Analyzers.NoPublicConstructorOnEntity.NoPublicConstructorOnEntityAnalyzer>;


namespace CleanArchitecture.Core.Analyzers.Tests;

public class NoPublicConstructorOnEntityTests : AnalyzerTestBase
{
    private static string GetSource(string baseClassName, string ctorType = "public") => $@"
using {typeof(Entity).Namespace};

public class TestClass : {baseClassName}
{{
    {ctorType} TestClass()
    {{
    }}
}}
";

    [Fact]
    public async Task Public_constructor_has_diagnostic()
    {
        var expectedDiagnostic = DiagnosticResult
            .CompilerWarning(NoPublicConstructorOnEntityAnalyzer.DiagnosticId)
            .WithMessageFormat(NoPublicConstructorOnEntityAnalyzer.MessageFormat)
            .WithLocation(Location, 6, 5);


        await Verify.VerifyAnalyzerAsync(GetSource(nameof(Entity)), expectedDiagnostic);
    }

    [Theory]
    [InlineData("internal")]
    [InlineData("protected")]
    [InlineData("private")]
    [InlineData("private protected")]
    public async Task Constructors_with_no_diagnostic(string ctorType)
    {
        await Verify.VerifyAnalyzerAsync(GetSource(nameof(Entity), ctorType));
    }

    [Fact]
    public async Task AggregateRoot_has_no_diagnostic()
    {
        await Verify.VerifyAnalyzerAsync(GetSource(nameof(AggregateRoot)));
    }
}