using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CleanArchitecture.Core.Analyzers;

// TODO: Figure out a way to put this in a separate project so that it can be reused with source generators
// Currently, for some reason not known to me, I cannot put this static class into 
// CleanArchitecture.Core.Roslyn.Helpers and reference it from this project and use in my diagnostics...
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
}