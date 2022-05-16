using System.Threading.Tasks;
using CleanArchitecture.Core.Analyzers.EntitiesMustBePartial;
using CleanArchitecture.Core.Entities;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using Verify =
    CleanArchitecture.Core.Analyzers.Tests.Verifiers.CsCodeFixVerifier<
        CleanArchitecture.Core.Analyzers.EntitiesMustBePartial.EntitiesMustBePartialAnalyzer,
        CleanArchitecture.Core.Analyzers.EntitiesMustBePartial.EntitiesMustBePartialCodeFixProvider
    >;


namespace CleanArchitecture.Core.Analyzers.Tests;

public class EntitiesMustBePartialTests : AnalyzerTestBase
{
    private static string AddDummyClass(bool yes) => yes
        ? @"
public abstract class DummyClass {}
"
        : "";

    private static string GetSource(bool inheritFromDummyClass = false) => $@"
using {typeof(Entity).Namespace};
{AddDummyClass(inheritFromDummyClass)}
public class TestClass : {(inheritFromDummyClass ? "DummyClass" : nameof(AggregateRoot))}
{{
    internal TestClass()
    {{
    }}
}}
";

    private static string GetFixedSource() => $@"
using {typeof(Entity).Namespace};

public partial class TestClass : {nameof(AggregateRoot)}
{{
    internal TestClass()
    {{
    }}
}}
";

    [Fact]
    public async Task Non_partial_entities_have_diagnostic_and_code_fix()
    {
        var expectedDiagnostic = DiagnosticResult
            .CompilerError(EntitiesMustBePartialAnalyzer.DiagnosticId)
            .WithMessageFormat(EntitiesMustBePartialAnalyzer.MessageFormat)
            .WithLocation(Location, 4, 1);
        
        await Verify.VerifyCodeFixAsync(GetSource(), expectedDiagnostic, GetFixedSource());
    }

    [Fact]
    public async Task No_diagnostic_when_not_entity()
    {
        await Verify.VerifyAnalyzerAsync(GetSource(true));
    }
}