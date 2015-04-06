using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NotQuiteLisp.Parser;
using ANTLR4.ParserHelpers;
using NotQuiteLisp.Parser;

namespace NotQuiteLisp.Visitors
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    using NotQuiteLisp.AST;

    public class ParseTreeConverter : NQLBaseVisitor<AstNode>
    {
        public override AstNode VisitMap(NotQuiteLisp.Parser.NQLParser.MapContext context)
        {
            var childNodes = GetChildAstNodes(context);

            return new MapNode(childNodes.Cast<KeyValuePairNode>().Where(node => node != null));
        }

        public override AstNode VisitKeyValuePair(NotQuiteLisp.Parser.NQLParser.KeyValuePairContext context)
        {
            var pair = context;
            var children = pair.Children().ToArray();
            if (children.Length < 2)
                return base.VisitKeyValuePair(context);

            var keyContext = children.First();
            var valueContext = children.Last();

            var key = Visit(keyContext) as AtomNode;
            var value = Visit(valueContext) as ElementNode;

            if (key == null || value == null)
                return null;

            return new KeyValuePairNode(key, value);
        }

        public override AstNode VisitCompileUnit(NQLParser.CompileUnitContext context)
        {
            var childNodes = GetChildAstNodes(context);

            var rootNode = new RootNode(childNodes);
            return rootNode;
        }

        public override AstNode VisitSet(NotQuiteLisp.Parser.NQLParser.SetContext context)
        {
            var children = GetChildAstNodes(context);

            var setNode = new SetNode(children);
            return setNode;
        }

        public override AstNode VisitVector(NotQuiteLisp.Parser.NQLParser.VectorContext context)
        {
            var children = GetChildAstNodes(context);

            var vectorNode = new VectorNode(children);
            return vectorNode;
        }

        public override AstNode VisitAtom(NotQuiteLisp.Parser.NQLParser.AtomContext context)
        {
            var children = context.Children().ToArray();

            if (children.Length < 1)
                return base.VisitAtom(context);

            var firstChild = children.First();
            return Visit(firstChild);
        }

        public override AstNode VisitList(NotQuiteLisp.Parser.NQLParser.ListContext context)
        {
            var children = GetChildAstNodes(context);

            var listNode = new ListNode(children);
            return listNode;
        }

        public override AstNode VisitQuotedList(NotQuiteLisp.Parser.NQLParser.QuotedListContext context)
        {
            var children = context.Children().ToArray();
            if (children.Count() < 2)
                return base.VisitQuotedList(context);

            var lastChild = children.Last();
            var listNode = Visit(lastChild) as ListNode;

            return listNode != null ? new QuotedListNode(listNode) : base.VisitQuotedList(context);
        }

        public override AstNode VisitTerminal(Antlr4.Runtime.Tree.ITerminalNode node)
        {
            var payload = (CommonToken)node.Payload;

            var ruleType = payload.Type;
            if (ruleType == NQLParser.OPERATOR)
                return new OperatorNode(payload.Text);

            if (ruleType == NQLParser.STRING)
                return new StringNode(payload.Text);

            if (ruleType == NQLParser.NUMBER)
                return new NumberNode(payload.Text);

            if (ruleType == NQLParser.SYMBOL)
            {
                if (payload.Text == "nil") 
                    return new NilNode();

                return new SymbolNode(payload.Text);
            }                

            if (ruleType == NQLParser.KEYWORD)
                return new KeywordNode(payload.Text);

            return base.VisitTerminal(node);
        }

        private IEnumerable<AstNode> GetChildAstNodes(IParseTree context)
        {
            return GetChildAstNodes(context, this.Visit);
        }

        private static IEnumerable<AstNode> GetChildAstNodes(IParseTree context, Func<IParseTree, AstNode> visitFunc)
        {
            return context.Children()
                .Select(visitFunc)
                .Where(c => c != null)
                .ToArray();
        }
    }
}
