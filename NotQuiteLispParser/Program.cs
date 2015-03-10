using System;
using System.Linq;
using System.Text;

namespace NotQuiteLispParser
{
    using ANTLR4.ParserHelpers;

    public class TreeBuilder<TStrategy> : TreeBuilder
        where TStrategy : ITreeBuilderStrategy, new()
    {
        public TreeBuilder()
            : base(new TStrategy())
        {
        }

        public TreeBuilder(ITreeBuilderStrategy strategy)
            : base(strategy)
        {
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //var inputText = @"(add 1 2 (mul 3 4))";

            var inputText = "(+ 1 2)";
            var treeBuilder = new TreeBuilder<NqlTreeBuilderStrategy>();
            var parseTree = treeBuilder.CreateParseTree(inputText);

            var visitor = new NqlVisitor();
            var tree = visitor.Visit(parseTree);
            //var expectedTree = new ListNode(new SymbolNode("add"),
            //    new NumberNode("1"), new NumberNode("2"));

            return;
        }
    }
}
