namespace NotQuiteLisp.AST
{
    using System;
    using System.Collections.Generic;

    using NotQuiteLisp.AST.Interfaces;

    public abstract class Node<T> : INode<T>
    {
        private readonly Guid _nodeId = Guid.NewGuid();

        private readonly List<INode<T>> _childNodes;

        protected Node()
            : this(new List<Node<T>>())
        {
        }

        protected Node(IEnumerable<INode<T>> childNodes)
        {
            _childNodes = new List<INode<T>>(childNodes);
        }

        public Guid NodeId
        {
            get { return _nodeId; }
        }

        public virtual IEnumerable<INode<T>> Children
        {
            get
            {
                return _childNodes;
            }
        }

        public abstract INode<T> Clone();
    }
}