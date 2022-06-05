// using System;
// using System.Linq;
// using CleanArchitecture.Core.Events;
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp;
// using Xunit;
//
// namespace CleanArchitecture.Core.SourceGenerators.Tests;
//
// public class IntegrationEventFromDomainEventGeneratorTests
// {
//     private static string? GetGeneratedOutput(string sourceCode)
//     {
//         var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
//         var references = AppDomain.CurrentDomain.GetAssemblies()
//             .Where(assembly => !assembly.IsDynamic)
//             .Select(assembly => MetadataReference
//                 .CreateFromFile(assembly.Location))
//             .Cast<MetadataReference>();
//
//         var compilation = CSharpCompilation.Create("SourceGeneratorTests",
//             new[] {syntaxTree},
//             references,
//             new(OutputKind.DynamicallyLinkedLibrary));
//
//         // Source Generator to test
//         var generator = new IntegrationEventFromDomainEventGenerator();
//
//         CSharpGeneratorDriver.Create(generator)
//             .RunGeneratorsAndUpdateCompilation(compilation,
//                 out var outputCompilation,
//                 out var diagnostics);
//
//         // optional
//         Assert.Empty(diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));
//
//         return outputCompilation.SyntaxTrees.Skip(1).LastOrDefault()?.ToString();
//     }
//
//     [Fact]
//     public void Adds_additional_interface_when_IIntegrationEventDomEv()
//     {
//         var input = $@"
// using {typeof(DomainEvent).Namespace};
//
// namespace DemoTests
// {{
//     public class TestDomainEvent : {nameof(DomainEvent)} 
//     {{
//     }}
//
//     public partial class TestIntegrationEvent : {nameof(IIntegrationEvent)}<TestDomainEvent>
//     {{
//     }}
// }}
// ";
//
//         var output = $@" // Auto-generated code
// using {typeof(DomainEvent).Namespace};
//
// namespace DemoTests
// {{
//     public partial class TestIntegrationEvent : IIntegrationEvent<TestDomainEvent, TestIntegrationEvent>
//     {{
//     }}
// }}
// ";
//
//         var generatedOutput = GetGeneratedOutput(input);
//         Assert.Equal(output, generatedOutput);
//     }
// }