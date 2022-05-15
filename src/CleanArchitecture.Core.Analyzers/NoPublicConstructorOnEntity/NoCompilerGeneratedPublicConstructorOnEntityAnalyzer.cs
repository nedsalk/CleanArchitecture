using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CleanArchitecture.Core.Analyzers.NoPublicConstructorOnEntity;

public class NoCompilerGeneratedPublicConstructorOnEntityAnalyzer
    // : DiagnosticAnalyzer
{
    // public const string DiagnosticId = nameof(NoCompilerGeneratedPublicConstructorOnEntityAnalyzer);
    //
    // public const string Title = "Entities cannot have compiler-generated public constructors";
    //
    // public const string MessageFormat = "Entities cannot have compiler-generated public constructors, only AggregateRoots can.";
    //
    // private const string Description = Title;
    //
    // private const string Category = "Usage";
    //
    // private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat,
    //     Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);
    //
    // public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
    //
    // public override void Initialize(AnalysisContext context)
    // {
    //     context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
    //     context.EnableConcurrentExecution();
    //     context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ConstructorDeclaration);
    // }
    //
    // private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    // {
    //     var ctorDeclaration = (ConstructorDeclarationSyntax) context.Node;
    //     if (!MatchesPreconditions(context, ctorDeclaration)) return;
    //
    //     context.ReportDiagnostic(Diagnostic.Create(Rule, ctorDeclaration.GetLocation()));
    // }
    //
    // private static bool MatchesPreconditions(SyntaxNodeAnalysisContext context,
    //     ConstructorDeclarationSyntax ctorDeclaration)
    // {
    //     var isPublicCtor = ctorDeclaration.Modifiers.Any(x => x.IsKind(SyntaxKind.PublicKeyword));
    //
    //     if (!isPublicCtor) return false;
    //
    //     var classDeclaration = (ClassDeclarationSyntax) ctorDeclaration.Parent!;
    //
    //     return !classDeclaration.Is("AggregateRoot", context.SemanticModel) &&
    //            classDeclaration.Is("Entity", context.SemanticModel);
}