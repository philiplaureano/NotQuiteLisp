using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    using NotQuiteLisp.AST.Interfaces;

    public class TreeMap
    {
        private readonly ConcurrentDictionary<INode<AstNode>, INode<AstNode>> _parentMap = new ConcurrentDictionary<INode<AstNode>, INode<AstNode>>();

        public INode<AstNode> GetParentFor(INode<AstNode> child)
        {
            return _parentMap.ContainsKey(child) ? _parentMap[child] : null;
        }

        public void SetParentFor(INode<AstNode> child, INode<AstNode> parent)
        {
            if (child == null)
                throw new ArgumentNullException("child");

            _parentMap[child] = parent;
        }

        public LinkedList<INode<AstNode>> GetRootAncestryFor(INode<AstNode> descendantNode)
        {
            var results = new LinkedList<INode<AstNode>>();
            results.AddFirst(descendantNode);

            var parentNode = GetParentFor(descendantNode);
            while (parentNode != null)
            {
                results.AddLast(parentNode);
                parentNode = GetParentFor(parentNode);
            }

            return results;
        }
    }
}