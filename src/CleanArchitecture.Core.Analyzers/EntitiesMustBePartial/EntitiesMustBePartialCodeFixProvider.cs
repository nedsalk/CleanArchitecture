using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CleanArchitecture.Core.Analyzers.EntitiesMustBePartial;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EntitiesMustBePartialCodeFixProvider)), Shared]
public class EntitiesMustBePartialCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(EntitiesMustBePartialAnalyzer.DiagnosticId);

    private const string ToPartial = "Convert to partial";

    // public sealed override FixAllProvider GetFixAllProvider()
    // {
    //     // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
    //     return WellKnownFixAllProviders.BatchFixer;
    // }
    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        var diagnostic = context.Diagnostics.First();

        // Find the type declaration identified by the diagnostic.
        var declaration = root!.FindToken(diagnostic.Location.SourceSpan.Start).Parent!.AncestorsAndSelf()
            .OfType<ClassDeclarationSyntax>().First();

        context.RegisterCodeFix(CodeAction.Create(
            title: ToPartial,
            equivalenceKey: nameof(ToPartial),
            createChangedDocument: c => ChangeToPartial(context.Document, declaration, c)
        ), diagnostic);
    }

    private static async Task<Document> ChangeToPartial(Document document, ClassDeclarationSyntax declaration,
        CancellationToken cancellationToken)
    {
        var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);

        var newRoot = oldRoot!.ReplaceNode(declaration,
            declaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword)));

        return document.WithSyntaxRoot(newRoot);
    }
}