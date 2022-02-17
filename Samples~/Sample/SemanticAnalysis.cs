using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEngine;
using UnityEditor;

// sample derived from
// https://github.com/dotnet/roslyn/blob/main/docs/wiki/Getting-Started-C%23-Semantic-Analysis.md

public class SemanticAnalysis : MonoBehaviour
{
    [MenuItem("Tools/Test Roslyn Samples/Semantic Analysis")]
    static void Test()
    {
        SyntaxTree tree = CSharpSyntaxTree.ParseText(
@" using System;
using System.Collections.Generic;
using System.Text;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}");

        var root = (CompilationUnitSyntax)tree.GetRoot();

        var compilation = CSharpCompilation.Create("HelloWorld")
                                           .AddReferences(
                                                MetadataReference.CreateFromFile(
                                                    typeof(object).Assembly.Location))
                                           .AddSyntaxTrees(tree);

        var model = compilation.GetSemanticModel(tree);

        var nameInfo = model.GetSymbolInfo(root.Usings[0].Name);

        var systemSymbol = (INamespaceSymbol)nameInfo.Symbol;

        foreach (var ns in systemSymbol.GetNamespaceMembers())
        {
            //Console.WriteLine(ns.Name);
            Debug.Log(ns.Name);
        }

        var helloWorldString = root.DescendantNodes()
                                   .OfType<LiteralExpressionSyntax>()
                                   .First();

        var literalInfo = model.GetTypeInfo(helloWorldString);

        var stringTypeSymbol = (INamedTypeSymbol)literalInfo.Type;

        //Console.Clear();
        foreach (var name in (from method in stringTypeSymbol.GetMembers()
                                                          .OfType<IMethodSymbol>()
                              where method.ReturnType.Equals(stringTypeSymbol) &&
                                    method.DeclaredAccessibility ==
                                               Accessibility.Public
                              select method.Name).Distinct())
        {
            //Console.WriteLine(name);
            Debug.Log(name);
        }
    }
}
