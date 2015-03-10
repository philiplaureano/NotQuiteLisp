namespace NotQuiteLispParser
{
    using System.Collections.Generic;

    public abstract class AstNode
    {
        private readonly List<AstNode> _childNodes;

        protected AstNode()
            : this(new List<AstNode>())
        {
        }

        protected AstNode(IEnumerable<AstNode> childNodes)
        {
            this._childNodes = new List<AstNode>(childNodes);
        }

        public IEnumerable<AstNode> Children
        {
            get
            {
                return this._childNodes;
            }
        }
    }
}