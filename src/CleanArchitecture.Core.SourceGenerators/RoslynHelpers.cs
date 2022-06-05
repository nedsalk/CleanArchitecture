using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CleanArchitecture.Core.SourceGenerators;

public static class RoslynHelpers
{
    public static bool Is(this ClassDeclarationSyntax classDeclaration, string name, SemanticModel semanticModel,
        int? depth = null)
    {
        var classTypeSymbol = (ITypeSymbol) semanticModel.GetDeclaredSymbol(classDeclaration)!;
        var baseClassNames = classTypeSymbol.GetBaseTypesAndThis(depth)
            .Select(x => x.Name);

        return baseClassNames.Contains(name);
    }

    private static IEnumerable<ITypeSymbol> GetBaseTypesAndThis(this ITypeSymbol? type, int? depth = null)
    {
        var current = type;
        while (current != null && depth != 0)
        {
            yield return current;
            current = current.BaseType;
            if (depth.HasValue)
            {
                depth -= 1;
            }
        }
    }

    public static T? GetParentNode<T>(this SyntaxNode node) where T : class =>
        node.Parent switch
        {
            null => null,
            T parent => parent,
            _ => GetParentNode<T>(node.Parent)
        };
}