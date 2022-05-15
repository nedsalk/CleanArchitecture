using Microsoft.CodeAnalysis.CSharp.Testing.XUnit;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CleanArchitecture.Core.Analyzers.Tests.Verifiers;

public abstract class CSharpAnalyzerVerifier<TAnalyzer> : AnalyzerVerifier<TAnalyzer>
    where TAnalyzer : DiagnosticAnalyzer, new()
{
}