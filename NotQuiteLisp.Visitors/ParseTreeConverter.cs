using ANTLR4.ParserHelpers;
using NotQuiteLisp.AST.Interfaces;
using NotQuiteLisp.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NotQuiteLisp.Visitors
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    using NotQuiteLisp.AST;

    public class ParseTreeConverter : NQLBaseVisitor<INode<AstNode>>
    {
        public override INode<AstNode> VisitMap(NotQuiteLisp.Parser.NQLParser.MapContext context)
        {
            var childNodes = GetChildAstNodes(context);

            return new MapNode(childNodes.OfType<KeyValuePairNode>());
        }

        public override INode<AstNode> VisitKeyValuePair(NotQuiteLisp.Parser.NQLParser.KeyValuePairContext context)
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

        public override INode<AstNode> VisitCompileUnit(NQLParser.CompileUnitContext context)
        {
            var childNodes = GetChildAstNodes(context);

            var rootNode = new RootNode(childNodes);
            return rootNode;
        }

        public override INode<AstNode> VisitSet(NotQuiteLisp.Parser.NQLParser.SetContext context)
        {
            var children = GetChildAstNodes(context);

            var setNode = new SetNode(children);
            return setNode;
        }

        public override INode<AstNode> VisitVector(NotQuiteLisp.Parser.NQLParser.VectorContext context)
        {
            var children = GetChildAstNodes(context);

            var vectorNode = new VectorNode(children);
            return vectorNode;
        }

        public override INode<AstNode> VisitAtom(NotQuiteLisp.Parser.NQLParser.AtomContext context)
        {
            var children = context.Children().ToArray();

            if (children.Length < 1)
                return base.VisitAtom(context);

            var firstChild = children.First();
            return Visit(firstChild);
        }

        public override INode<AstNode> VisitList(NotQuiteLisp.Parser.NQLParser.ListContext context)
        {
            var children = GetChildAstNodes(context);

            var listNode = new ListNode(children);
            return listNode;
        }

        public override INode<AstNode> VisitQuotedList(NotQuiteLisp.Parser.NQLParser.QuotedListContext context)
        {
            var children = context.Children().ToArray();
            if (children.Count() < 2)
                return base.VisitQuotedList(context);

            var lastChild = children.Last();
            var listNode = Visit(lastChild) as ListNode;

            return listNode != null ? new QuotedListNode(listNode) : base.VisitQuotedList(context);
        }

        public override INode<AstNode> VisitTerminal(Antlr4.Runtime.Tree.ITerminalNode node)
        {
            var payload = (CommonToken)node.Payload;

            var ruleType = payload.Type;
            var text = payload.Text;

            if (ruleType == NQLParser.OPERATOR)
                return new OperatorNode(text);

            if (ruleType == NQLParser.STRING)
            {
                var unwrappedText = text.TrimStart('\"').TrimEnd('\"');
                return new StringNode(unwrappedText);
            }                

            if (ruleType == NQLParser.NUMBER)
                return new NumberNode(text);

            if (ruleType == NQLParser.SYMBOL || ruleType == NQLParser.ALPHA)
            {
                if (text == "nil")
                    return new NilNode();

                if (text == "true")
                    return new TrueNode();

                if (text == "false")
                    return new FalseNode();

                return new SymbolNode(text);
            }

            if (ruleType == NQLParser.KEYWORD)
                return new KeywordNode(text);

            return base.VisitTerminal(node);
        }

        private IEnumerable<INode<AstNode>> GetChildAstNodes(IParseTree context)
        {
            return GetChildAstNodes(context, Visit);
        }

        private static IEnumerable<INode<AstNode>> GetChildAstNodes(IParseTree context, Func<IParseTree, INode<AstNode>> visitFunc)
        {
            return context.Children()
                .Select(visitFunc)
                .Where(c => c != null)
                .ToArray();
        }
    }
}
