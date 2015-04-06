namespace NotQuiteLisp.AST
{
    using System.Collections.Generic;

    public abstract class ElementNode : AstNode
    {
        protected ElementNode()
        {
        }

        protected ElementNode(IEnumerable<AstNode> childNodes)
            : base(childNodes)
        {
        }
    }
}