using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace NotQuiteLisp.Core
{
    using NotQuiteLisp.AST.Interfaces;
    public class TreeMap<TItem>
    {
        private readonly ConcurrentDictionary<INode<TItem>, INode<TItem>> _parentMap = new ConcurrentDictionary<INode<TItem>, INode<TItem>>();

        private readonly ConcurrentDictionary<INode<TItem>, IEnumerable<INode<TItem>>> _siblingMap =
            new ConcurrentDictionary<INode<TItem>, IEnumerable<INode<TItem>>>();

        public INode<TItem> GetParentFor(INode<TItem> child)
        {
            return _parentMap.ContainsKey(child) ? _parentMap[child] : null;
        }

        public IEnumerable<INode<TItem>> GetSiblingsFor(INode<TItem> node)
        {
            if (_siblingMap.ContainsKey(node))
                return _siblingMap[node];

            return new INode<TItem>[0];
        }

        public void SetParentFor(INode<TItem> child, INode<TItem> parent)
        {
            if (child == null)
                throw new ArgumentNullException("child");

            _parentMap[child] = parent;

            if (parent == null)
                return;

            // Map the siblings
            var siblings = parent.Children.Where(c => c.NodeId != child.NodeId);
            _siblingMap[child] = siblings;
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