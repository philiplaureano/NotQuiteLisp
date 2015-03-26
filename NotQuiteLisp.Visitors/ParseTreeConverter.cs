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

    using NotQuiteLisp.AST;

    public class ParseTreeConverter : NQLBaseVisitor<AstNode>
    {
        public override AstNode VisitCompileUnit(NQLParser.CompileUnitContext context)
        {
            var childNodes = new List<AstNode>();
            foreach (var child in context.Children())
            {
                var childNode = Visit(child);
                if (childNode != null)
                    childNodes.Add(childNode);

            }

            var rootNode = new RootNode(childNodes);
            return rootNode;
        }
        public override AstNode VisitVector(NotQuiteLisp.Parser.NQLParser.VectorContext context)
        {
            var children = context.Children()
                .Select(c => this.Visit(c))
                .Where(c => c != null)
                .ToArray();

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
            var children = context.Children()
                .Select(c => this.Visit(c))
                .Where(c => c != null)
                .ToArray();

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
                return new SymbolNode(payload.Text);

            return base.VisitTerminal(node);
        }
    }
}
