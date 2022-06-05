using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using Verify =
    CleanArchitecture.Core.Analyzers.Tests.Verifiers.CsAnalyzerVerifier<
        CleanArchitecture.Core.Analyzers.AccessToNonPublicSettersNotAllowedAnalyzer>;

namespace CleanArchitecture.Core.Analyzers.Tests;

public class AccessToNonPublicSettersNotAllowedTests : AnalyzerTestBase
{
    private static string GetSource(string modifier = "public") => $@"
using {typeof(Entity).Namespace};

public class TestClass : {nameof(Entity)}
{{
    internal string TestProperty {{get; private set;}}

    public void SetSomething(string s) {{
        TestProperty = s;
    }}
}}
";

    [Fact]
    public async Task Works_for_properties()
    {
        var expectedDiagnostic = DiagnosticResult
            .CompilerWarning(AccessToNonPublicSettersNotAllowedAnalyzer.DiagnosticId)
            .WithMessageFormat(AccessToNonPublicSettersNotAllowedAnalyzer.MessageFormat)
            .WithLocation(Location, 9, 9);
        await Verify.VerifyAnalyzerAsync(GetSource(), expectedDiagnostic);
    }

    [Fact]
    public async Task No_diagnostic_for_aggregate_roots()
    {
        
    }
}