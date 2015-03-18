using System.Collections;
using System.Collections.Generic;

namespace ANTLR4.ParserHelpers
{
    using Antlr4.Runtime.Tree;

    public static class ParseTreeExtensions
    {
        public static void WalkWith(this IParseTree parseTree, IParseTreeListener listener)
        {
            var parseTreeWalker = ParseTreeWalker.Default;
            parseTree.WalkWith(listener, parseTreeWalker);
        }

        public static void WalkWith(this IParseTree parseTree, IParseTreeListener listener, ParseTreeWalker walker)
        {
            if (parseTree == null || listener == null || walker == null)
                return;

            walker.Walk(listener, parseTree);
        }

        public static IEnumerable<IParseTree> Children(this IParseTree parseTree)
        {
            var childCount = parseTree.ChildCount;
            for (var i = 0; i < childCount; i++)
            {
                yield return parseTree.GetChild(i);
            }
        }

        public static IEnumerable<IParseTree> Descendants(this IParseTree parseTree)
        {
            var results = new List<IParseTree>();
            AddDecendants(parseTree, results);

            return results;
        }

        private static void AddDecendants(IParseTree parseTree, IList<IParseTree> results)
        {
            foreach (var child in parseTree.Children())
            {
                AddDecendants(child, results);
                results.Add(child);
            }
        }
    }
}