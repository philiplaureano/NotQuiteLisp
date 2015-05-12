using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    using NotQuiteLisp.AST.Interfaces;
    public class TreeMap<TItem>
    {
        private readonly ConcurrentDictionary<INode<TItem>, INode<TItem>> _parentMap = new ConcurrentDictionary<INode<TItem>, INode<TItem>>();

        public INode<TItem> GetParentFor(INode<TItem> child)
        {
            return _parentMap.ContainsKey(child) ? _parentMap[child] : null;
        }

        public void SetParentFor(INode<TItem> child, INode<TItem> parent)
        {
            if (child == null)
                throw new ArgumentNullException("child");

            _parentMap[child] = parent;
        }

        public LinkedList<INode<TItem>> GetRootAncestryFor(INode<TItem> descendantNode)
        {
            var results = new LinkedList<INode<TItem>>();
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