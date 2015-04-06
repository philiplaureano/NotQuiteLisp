namespace NotQuiteLisp.AST
{
    using System.Collections.Generic;

    public abstract class AtomNode : ElementNode
    {
        protected AtomNode()
        {
        }

        protected AtomNode(IEnumerable<AstNode> childNodes)
            : base(childNodes)
        {
        }
    }
}