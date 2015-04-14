using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SyntaxTreeAnalyzer
{
    public class SyntaxTreeAnalyzer
    {
        public SyntaxTreeAnalyzer()
        {
        }
            
        public static void GetSyntaxTree(string filePath)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseFile(filePath);
        }
    }
}
