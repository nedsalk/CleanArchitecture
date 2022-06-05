using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CleanArchitecture.Core.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class AccessToNonPublicSettersNotAllowedAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = nameof(AccessToNonPublicSettersNotAllowedAnalyzer);

    public const string Title = "In entities, public methods should not access non-public setters.";

    public const string MessageFormat = Title;

    private const string Description = Title;

    private const string Category = "Usage";

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat,
        Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.SimpleAssignmentExpression);
    }

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var expressionStatementSyntax = (AssignmentExpressionSyntax) context.Node;
        if (!MatchesPreconditions(context, expressionStatementSyntax)) return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, expressionStatementSyntax.GetLocation()));
    }


    private static bool MatchesPreconditions(SyntaxNodeAnalysisContext context,
        AssignmentExpressionSyntax assignmentExpression)
    {
        var isInPublicMethod = assignmentExpression.Ancestors().Any(x =>
            x is MethodDeclarationSyntax mds && mds.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)));

        if (!isInPublicMethod) return false;

        var isInEntity = assignmentExpression.Ancestors()
            .Any(x => x is ClassDeclarationSyntax cds && cds.Is("Entity", context.SemanticModel));

        if (!isInEntity) return false;

        var property = (IPropertySymbol) context.SemanticModel.GetSymbolInfo(assignmentExpression.Left).Symbol!;

        return property.SetMethod is not null &&
               property.SetMethod.DeclaredAccessibility != Accessibility.Public;
    }
}