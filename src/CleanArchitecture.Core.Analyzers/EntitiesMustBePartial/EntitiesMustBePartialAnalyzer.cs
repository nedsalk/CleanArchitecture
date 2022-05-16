using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CleanArchitecture.Core.Analyzers.EntitiesMustBePartial;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class EntitiesMustBePartialAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = nameof(EntitiesMustBePartialAnalyzer);

    public const string Title = "Entities must be declared partial.";

    public const string MessageFormat = "Entities must be declared partial.";

    private const string Description = Title;

    private const string Category = "Usage";

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat,
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ClassDeclaration);
    }

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var ctorDeclaration = (ClassDeclarationSyntax) context.Node;
        if (!MatchesPreconditions(context, ctorDeclaration)) return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, ctorDeclaration.GetLocation()));
    }

    private static bool MatchesPreconditions(SyntaxNodeAnalysisContext context,
        ClassDeclarationSyntax classDeclaration)
    {
        return classDeclaration.Is("Entity", context.SemanticModel) &&
               !classDeclaration.Modifiers.Any(x => x.IsKind(SyntaxKind.PartialKeyword));
    }
}