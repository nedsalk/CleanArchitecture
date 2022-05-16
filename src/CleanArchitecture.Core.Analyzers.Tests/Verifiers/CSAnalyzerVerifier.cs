using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.CSharp.Testing.XUnit;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace CleanArchitecture.Core.Analyzers.Tests.Verifiers;

public abstract class CsAnalyzerVerifier<TAnalyzer> : AnalyzerVerifier<TAnalyzer>
    where TAnalyzer : DiagnosticAnalyzer, new()
{
    public new static async Task VerifyAnalyzerAsync(
        string source,
        params DiagnosticResult[] expected)
    {
        var test = new AnalyzerTest(source, expected);
        await test.RunAsync(CancellationToken.None);
    }

    private class AnalyzerTest : CSharpAnalyzerTest<TAnalyzer, XUnitVerifier>
    {
        public AnalyzerTest(
            string source,
            params DiagnosticResult[] expected)
        {
            TestCode = source;
            ExpectedDiagnostics.AddRange(expected);

            ReferenceAssemblies = new(
                "net6.0",
                new("Microsoft.NETCore.App.Ref", "6.0.0"),
                Path.Combine("ref", "net6.0"));


            TestState.AdditionalReferences.Add(typeof(Entity).Assembly);
        }
    }
}