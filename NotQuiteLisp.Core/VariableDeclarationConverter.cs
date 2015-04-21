using System;
using System.Collections.Concurrent;
using System.Linq;
using NotQuiteLisp.AST;
using NotQuiteLisp.Visitors;

namespace NotQuiteLisp.Core
{
    public class VariableDeclarationConverter : AstVisitor<AstNode>
    {
        private readonly ConcurrentBag<Guid> _declarations = new ConcurrentBag<Guid>();

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

        public AstNode Visit(ListNode node)
        {
            // Ignore empty lists
            if (!node.Children.Any())
                return node.Clone();

            // Ignore non-declaration nodes
            var symbolNode = node.Children.First() as SymbolNode;
            if (symbolNode == null || symbolNode.Symbol != "def" || !_declarations.Contains(node.NodeId))
                return node.Clone();

            var children = node.Children.ToArray();

            // A def statement needs two arguments: 
            // 1) the variable name as a symbol
            // 2) the value as an AST node
            if (children.Length < 3)
                return node.Clone();

            var variableNameNode = children[1] as SymbolNode;
            var valueNode = children[2];
            if (variableNameNode == null || valueNode == null)
                return node.Clone();

            var declaration = new VariableDeclarationNode(variableNameNode.Symbol, valueNode.Clone());
            return declaration;
        }

        public AstNode Visit(RootNode rootNode)
        {
            // Search for variable declarations
            var listNodes = rootNode.Descendants().Where(d => d.GetType() == typeof(ListNode)).Cast<ListNode>();
            var nonEmptyNodes = listNodes.Where(l => l.Children.Any());
            var declarationNodes = nonEmptyNodes.Where(l =>
            {
                var symbolNode = l.Children.First() as SymbolNode;
                return symbolNode != null && symbolNode.Symbol == "def";
            });

            var targetListNodeIds = declarationNodes.Select(node => node.NodeId);

            // Map all the declarations in the tree
            foreach (var currentId in targetListNodeIds)
            {
                _declarations.Add(currentId);
            }

            // Visit the child nodes
            var clonedChildren = rootNode.Children.Select(Visit);
            return new RootNode(clonedChildren);
        }
    }
}