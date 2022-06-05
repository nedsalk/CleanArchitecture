using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CleanArchitecture.Core.SourceGenerators;

using Microsoft.CodeAnalysis;

// [Generator]
public class IntegrationEventFromDomainEventGenerator
    // : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classOrRecordDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsNodeTargetForGeneration(s),
                transform: static (ctx, _) => (BaseTypeDeclarationSyntax) ctx.Node);

        IncrementalValueProvider<(Compilation, ImmutableArray<BaseTypeDeclarationSyntax>)> compilationAndClasses =
            context.CompilationProvider.Combine(classOrRecordDeclarations.Collect());


        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Item2, spc));
    }


    private static bool IsNodeTargetForGeneration(SyntaxNode node)
        => node is ClassDeclarationSyntax
           {
               BaseList.Types.Count: >= 1
           } c && IsValid(c) ||
           node is RecordDeclarationSyntax
           {
               BaseList.Types.Count: >= 1
           } r && IsValid(r);

    private static bool IsValid(BaseTypeDeclarationSyntax node)
    {
        return node.BaseList!.Types.Any(t =>
            t.Type is GenericNameSyntax {Identifier.Text: "IIntegrationEvent", TypeArgumentList.Arguments.Count: 1});
    }


    private static void Execute(ImmutableArray<BaseTypeDeclarationSyntax> nodes, SourceProductionContext context)
    {
        if (nodes.IsDefaultOrEmpty) return;

        var distinctNodes = nodes.Distinct().ToList();

        distinctNodes.ForEach(node =>
        {
            var sourceCode = GenerateSourceCode(node);

            context.AddSource($"{node.Identifier.Text}.g.cs", sourceCode);
        });
    }

    private static string GenerateSourceCode(BaseTypeDeclarationSyntax node)
    {
        var namespaceText = GetNamespaceText(node);
        var domainEventName = GetDomainEventName(node);
        var classOrRecord = node.DescendantTokens()
            .First(x => x.IsKind(SyntaxKind.ClassKeyword) || x.IsKind(SyntaxKind.RecordKeyword)).Text;

        var source = $@" // Auto-generated code
using CleanArchitecture.Core.Events;

{namespaceText}
{{
    public partial {classOrRecord} {node.Identifier.Text} : IIntegrationEvent<{domainEventName}, {node.Identifier.Text}>
    {{
    }}
}}
";

        return source;
    }

    private static string GetDomainEventName(BaseTypeDeclarationSyntax node)
    {
        var integrationEventSyntax = (GenericNameSyntax) node.BaseList!.Types.First(t =>
                t.Type is GenericNameSyntax {Identifier.Text: "IIntegrationEvent", TypeArgumentList.Arguments.Count: 1})
            .Type;

        return integrationEventSyntax.TypeArgumentList.Arguments.First().TryGetInferredMemberName()!;
    }

    private static string GetNamespaceText(SyntaxNode node)
    {
        var fileScopedNamespaceDeclaration = node.Parent as FileScopedNamespaceDeclarationSyntax;
        var namespaceDeclaration = node.Parent as NamespaceDeclarationSyntax;

        var nameSyntax = fileScopedNamespaceDeclaration?.Name ?? namespaceDeclaration!.Name;

        return $"namespace {nameSyntax.ToString()}";
    }
}