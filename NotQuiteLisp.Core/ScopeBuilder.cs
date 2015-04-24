using System;
using System.Linq;
using NotQuiteLisp.AST;
using NotQuiteLisp.Visitors;

namespace NotQuiteLisp.Core
{
    public class ScopeBuilder : AstVisitor<AstNode>
    {
        public override AstNode Visit(AstNode subject)
        {
            if (subject == null)
                throw new ArgumentNullException("subject");

            try
            {
                return Visit(subject, true);
            }
            catch (VisitorMethodNotFoundException)
            {
                // Clone the node if there is no suitable visitor found
                return subject.Clone();
            }
        }

        public AstNode Visit(SymbolNode node)
        {
            return new ScopedNode(node, new AnonymousScope(null), delegate { return new AnonymousScope(null); });
        }

        public AstNode Visit(RootNode node)
        {
            var clonedChildren = node.Children.Select(Visit);

            var newNode = new RootNode(clonedChildren);
            return new ScopedNode(newNode, new GlobalScope(), delegate { return new GlobalScope(); });
        }
    }
}