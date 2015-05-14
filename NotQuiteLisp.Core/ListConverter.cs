using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NotQuiteLisp.AST;
using NotQuiteLisp.AST.Interfaces;
using NotQuiteLisp.Visitors;

namespace NotQuiteLisp.Core
{
    public abstract class ListConverter : AstVisitor<INode<AstNode>>
    {
        private readonly ConcurrentBag<Guid> _declarations = new ConcurrentBag<Guid>();
        
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
            if (!ShouldBeConverted(node, symbolNode)) 
                return new ListNode(clonedChildren);

            var children = node.Children.ToArray();

            // Validate the child nodes
            if (HasValidChildNodes(node, children)) 
                return CreateConvertedNode(node, clonedChildren);

            return new ListNode(clonedChildren);
        }

        public INode<AstNode> Visit(RootNode rootNode)
        {
            var targetListNodeIds = GetListNodeIds(rootNode);

            // Map all the declarations in the tree
            foreach (var currentId in targetListNodeIds)
            {
                _declarations.Add(currentId);
            }

            // Visit the child nodes
            var clonedChildren = rootNode.Children.Select(Visit);
            return new RootNode(clonedChildren);
        }

        protected abstract bool HasValidChildNodes(INode<AstNode> parent, INode<AstNode>[] children);

        protected abstract bool ShouldBeConverted(SymbolNode symbolNode);

        protected abstract INode<AstNode> CreateConvertedNode(INode<AstNode> originalNode, IEnumerable<INode<AstNode>> children);

        private static IEnumerable<Guid> GetListNodeIds(INode<AstNode> rootNode)
        {
            // Search for the lists that match the target symbol
            var listNodes = rootNode.Descendants().Where(d => d.GetType() == typeof(ListNode)).Cast<ListNode>();
            var nonEmptyNodes = listNodes.Where(l => l.Children.Any());
            var declarationNodes = nonEmptyNodes.Where(l =>
            {
                var symbolNode = l.Children.First() as SymbolNode;
                return symbolNode != null;
            });

            var targetListNodeIds = declarationNodes.Select(node => node.NodeId);
            return targetListNodeIds;
        }

        private bool ShouldBeConverted(INode<AstNode> node, SymbolNode symbolNode)
        {
            if (node == null || symbolNode == null)
                return false;

            return ShouldBeConverted(symbolNode) && _declarations.Contains(node.NodeId);
        }
    }
}