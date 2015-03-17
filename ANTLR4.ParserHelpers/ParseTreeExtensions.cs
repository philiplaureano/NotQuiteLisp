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
    }
}