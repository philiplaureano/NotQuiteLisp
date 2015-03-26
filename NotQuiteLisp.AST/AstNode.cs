using System;

namespace NotQuiteLisp.AST
{
    using System.Collections.Generic;

    public abstract class AstNode
    {
        private readonly Guid _nodeId = Guid.NewGuid();

        private readonly List<AstNode> _childNodes;

        protected AstNode()
            : this(new List<AstNode>())
        {
        }

        public Guid NodeId
        {
            get { return _nodeId; }
        }

        protected AstNode(IEnumerable<AstNode> childNodes)
        {
            this._childNodes = new List<AstNode>(childNodes);
        }

        public virtual IEnumerable<AstNode> Children
        {
            get
            {
                return this._childNodes;
            }
        }
    }
}