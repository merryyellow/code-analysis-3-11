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


public class NewFeatures : MonoBehaviour
{
    [MenuItem("Tools/Test Roslyn Samples/New Language Feature")]
    static void Test()
    {
        SyntaxTree tree = CSharpSyntaxTree.ParseText(
@"using System;
 
namespace TopLevel
{
    public record Person
    {
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
    };
}");

        var root = (CompilationUnitSyntax)tree.GetRoot();

        var visitor = new RecordVisitor();
        visitor.Visit(root);
    }
}

class RecordVisitor : CSharpSyntaxWalker
{
    public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
    {
        base.VisitRecordDeclaration(node);

        Debug.Log(node.Identifier.ValueText);
    }
}
