using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.CSharp.Testing.XUnit;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;


namespace CleanArchitecture.Core.Analyzers.Tests.Verifiers;

public class CsCodeFixVerifier<TAnalyzer, TCodeFix> : CodeFixVerifier<TAnalyzer, TCodeFix>
    where TCodeFix : CodeFixProvider, new() where TAnalyzer : DiagnosticAnalyzer, new()
{
    public new static async Task VerifyAnalyzerAsync(
        string source,
        params DiagnosticResult[] expected)
    {
        await CsAnalyzerVerifier<TAnalyzer>.VerifyAnalyzerAsync(source, expected);
    }


    public new static async Task VerifyCodeFixAsync(
        string source,
        string fixedSource)
    {
        var test = new CodeFixTest(source, fixedSource);
        await test.RunAsync(CancellationToken.None);
    }

    public new static async Task VerifyCodeFixAsync(
        string source,
        DiagnosticResult expected,
        string fixedSource)
    {
        var test = new CodeFixTest(source, fixedSource, expected);
        await test.RunAsync(CancellationToken.None);
    }

    private class CodeFixTest : CSharpCodeFixTest<TAnalyzer, TCodeFix, XUnitVerifier>
    {
        public CodeFixTest(
            string source,
            string fixedSource,
            params DiagnosticResult[] expected)
        {
            TestCode = source;
            FixedCode = fixedSource;
            ExpectedDiagnostics.AddRange(expected);

            ReferenceAssemblies = new(
                "net6.0",
                new("Microsoft.NETCore.App.Ref", "6.0.0"),
                Path.Combine("ref", "net6.0"));

            TestState.AdditionalReferences.Add(typeof(Entity).Assembly);
        }
    }
}