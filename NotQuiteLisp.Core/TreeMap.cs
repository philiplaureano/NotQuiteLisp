using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    public class TreeMap
    {
        private readonly ConcurrentDictionary<AstNode, AstNode> _parentMap = new ConcurrentDictionary<AstNode, AstNode>();

        public AstNode GetParentFor(AstNode child)
        {
            return _parentMap.ContainsKey(child) ? _parentMap[child] : null;
        }

        public void SetParentFor(AstNode child, AstNode parent)
        {
            if (child == null)
                throw new ArgumentNullException("child");

            _parentMap[child] = parent;
        }

        public LinkedList<AstNode> GetRootAncestryFor(AstNode descendantNode)
        {
            var results = new LinkedList<AstNode>();
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