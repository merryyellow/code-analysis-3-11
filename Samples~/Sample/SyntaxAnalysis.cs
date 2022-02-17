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
// https://github.com/dotnet/roslyn/blob/main/docs/wiki/Getting-Started-C%23-Syntax-Analysis.md

public class SyntaxAnalysis : MonoBehaviour
{
    [MenuItem("Tools/Test Roslyn Samples/Syntax Analysis")]
    static void Test()
    {
        SyntaxTree tree = CSharpSyntaxTree.ParseText(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
 
namespace TopLevel
{
    using Microsoft;
    using System.ComponentModel;
 
    namespace Child1
    {
        using Microsoft.Win32;
        using System.Runtime.InteropServices;
 
        class Foo { }
    }
 
    namespace Child2
    {
        using System.CodeDom;
        using Microsoft.CSharp;
 
        class Bar { }
    }
}");

        var root = (CompilationUnitSyntax)tree.GetRoot();

        var collector = new UsingCollector();
        collector.Visit(root);

        foreach (var directive in collector.Usings)
        {
            //Console.WriteLine(directive.Name);
            Debug.Log(directive.Name);
        }
    }
}

class UsingCollector : CSharpSyntaxWalker
{
    public readonly List<UsingDirectiveSyntax> Usings = new List<UsingDirectiveSyntax>();

    public override void VisitUsingDirective(UsingDirectiveSyntax node)
    {
        if (node.Name.ToString() != "System" &&
            !node.Name.ToString().StartsWith("System."))
        {
            this.Usings.Add(node);
        }
    }
}
