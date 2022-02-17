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
    public class Program
    {
        string str;

        public string GetName()
        {
            if (str is not null)
                return str;
            return "";
        }
	}
}");

        var root = (CompilationUnitSyntax)tree.GetRoot();

        var visitor = new NotPatternVisitor();
        visitor.Visit(root);
    }
}

class NotPatternVisitor : CSharpSyntaxWalker
{
    public override void VisitUnaryPattern(UnaryPatternSyntax node)
    {
        base.VisitUnaryPattern(node);

        if (node.OperatorToken.ValueText == "not")
        {
            Debug.Log("not pattern");
        }
    }
}
