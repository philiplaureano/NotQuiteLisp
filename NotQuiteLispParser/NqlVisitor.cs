using System;
using System.Linq;
namespace NotQuiteLispParser
{
    using System.Collections.Generic;

    using Antlr4.Runtime.Tree;

    public class NqlVisitor : NQLBaseVisitor<AstNode>
    {
        public override AstNode Visit(IParseTree tree)
        {
            return base.Visit(tree);
        }
        public override AstNode VisitCompileUnit(NQLParser.CompileUnitContext context)
        {
            var elements = context.element();
            var elementList = new List<AstNode>();
            foreach (var element in elements)
            {
                elementList.Add(this.VisitElement(element));
            }

            return new ListNode(elementList);
        }
        public override AstNode VisitList(NQLParser.ListContext context)
        {
            var elements = context.element();

            var nodes = new List<AstNode>();
            foreach (var element in elements)
            {
                var currentNode = VisitElement(element);
                nodes.Add(currentNode);
            }

            var listNode = new ListNode(nodes);
            return listNode;
        }
       
        public override AstNode VisitAtom(NQLParser.AtomContext context)
        {
            return base.VisitAtom(context);
        }
        public override AstNode VisitTerminal(ITerminalNode node)
        {
            var ruleNode = node.Parent;

            var symbol = node.Symbol;
            var nodeType = symbol.Type;

            var text = node.GetText();
            if (nodeType == NQLParser.STRING)
            {
                return new StringNode(text);
            }
            if (nodeType == NQLParser.SYMBOL)
            {
                return new SymbolNode(text);
            }

            if (nodeType == NQLParser.NUMBER)
            {
                return new NumberNode(text);
            }

            return base.VisitTerminal(node);
        }
    }
}