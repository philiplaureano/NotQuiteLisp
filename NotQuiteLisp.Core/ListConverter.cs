namespace NotQuiteLisp.Core
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using AST;

    using NotQuiteLisp.AST.Interfaces;

    using Visitors;

    public abstract class ListConverter : AstVisitor<INode<AstNode>>
    {
        private readonly ConcurrentBag<Guid> _declarations = new ConcurrentBag<Guid>();
        private readonly int _expectedParameterCount;

        protected ListConverter(int expectedParameterCount, string targetSymbol)
        {
            if (expectedParameterCount < 0)
                throw new ArgumentOutOfRangeException("expectedParameterCount");

            if (string.IsNullOrEmpty(targetSymbol))
                throw new ArgumentException("The target symbol cannot be a null or empty string.", "targetSymbol");

            _expectedParameterCount = expectedParameterCount;
            _targetSymbol = targetSymbol;
        }

        private readonly string _targetSymbol;

        public override INode<AstNode> Visit(INode<AstNode> subject)
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

        public INode<AstNode> Visit(ListNode node)
        {
            // Ignore empty lists
            if (!node.Children.Any())
                return node.Clone();

            var clonedChildren = node.Children.Select(Visit);

            // Ignore non-declaration nodes
            var symbolNode = node.Children.First() as SymbolNode;
            if (symbolNode == null || symbolNode.Symbol != _targetSymbol || !_declarations.Contains(node.NodeId))
                return new ListNode(clonedChildren);

            var children = node.Children.ToArray();

            // Validate the number of arguments
            if (children.Length != _expectedParameterCount + 1)
                return new ListNode(clonedChildren);

            return CreateConvertedNode(node, clonedChildren);
        }

        public INode<AstNode> Visit(RootNode rootNode)
        {
            var targetListNodeIds = GetListNodeIds(rootNode, _targetSymbol);

            // Map all the declarations in the tree
            foreach (var currentId in targetListNodeIds)
            {
                _declarations.Add(currentId);
            }

            // Visit the child nodes
            var clonedChildren = rootNode.Children.Select(Visit);
            return new RootNode(clonedChildren);
        }

        protected abstract INode<AstNode> CreateConvertedNode(INode<AstNode> originalNode, IEnumerable<INode<AstNode>> children);

        private static IEnumerable<Guid> GetListNodeIds(INode<AstNode> rootNode, string targetSymbol)
        {
            // Search for the lists that match the target symbol
            var listNodes = rootNode.Descendants().Where(d => d.GetType() == typeof(ListNode)).Cast<ListNode>();
            var nonEmptyNodes = listNodes.Where(l => l.Children.Any());
            var declarationNodes = nonEmptyNodes.Where(l =>
            {
                var symbolNode = l.Children.First() as SymbolNode;
                return symbolNode != null && symbolNode.Symbol == targetSymbol;
            });

            var targetListNodeIds = declarationNodes.Select(node => node.NodeId);
            return targetListNodeIds;
        }
    }
}