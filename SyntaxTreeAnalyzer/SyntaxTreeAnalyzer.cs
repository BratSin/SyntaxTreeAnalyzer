using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

        private static List<string> excludeDirectories = new List<string>() { "\\bin", "\\obj", "\\Properties" };
            
        public static string GetSyntaxTrees(string projectPath)
        {
            StringBuilder result = new StringBuilder();
            Dictionary<string, SyntaxTree> trees = getSyntaxTreesForFiles(projectPath);

            foreach(string key in trees.Keys)
            {
                result.AppendLine(String.Format("{0}\t{1}", key, trees[key].ToString()));
            }

            return result.ToString();
        }

        public static string GetTrees(string projectPath)
        {
            StringBuilder result = new StringBuilder();
            Dictionary<string, SyntaxTree> trees = getSyntaxTreesForFiles(projectPath);

            foreach(string k in trees.Keys)
            {
                getTree(trees[k].GetRoot(), result, 0);

                result.AppendLine();
            }

            return result.ToString();
        }

        private static void getTree(SyntaxNode node, StringBuilder buffer, int offset)
        {
            string result = new string('\t', offset);
            result += node.ToString();
            buffer.AppendLine(result);
            
            foreach(SyntaxNode n in node.ChildNodes())
            {
                getTree(n, buffer, offset + 1);
            }
        }

        public static string GetFilesFromProject(string projectPath)
        {
            return String.Join(Environment.NewLine, getFilesFromProject(projectPath).ToArray<string>());
        }

        private static List<string> getFilesFromProject(string projectPath)
        {
            List<string> result = new List<string>();

            string projectFolderPath = Path.GetDirectoryName(projectPath);

            foreach (string directory in Directory.GetDirectories(projectFolderPath, "*", SearchOption.AllDirectories)
                .Where(s => { foreach (string str in excludeDirectories) { if (s.Contains(str)) return false; } return true; })
                .Concat(new string[] { projectFolderPath }))
            {
                foreach(string file in Directory.GetFiles(directory, "*.cs", SearchOption.TopDirectoryOnly))
                {
                    result.Add(file);
                }  
            }

            return result;
        }
        
        private static Dictionary<string, SyntaxTree> getSyntaxTreesForFiles(string projectPath)
        {
            Dictionary<string, SyntaxTree> result = new Dictionary<string, SyntaxTree>();

            foreach(string file in getFilesFromProject(projectPath))
            {
                result.Add(file, CSharpSyntaxTree.ParseFile(file));
            }

            return result;
        }
    }

    
}
