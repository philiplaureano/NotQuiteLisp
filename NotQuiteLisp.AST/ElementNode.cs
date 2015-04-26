namespace NotQuiteLisp.AST
{
    using System.Collections.Generic;

    using NotQuiteLisp.AST.Interfaces;

    public abstract class ElementNode : AstNode
    {
        protected ElementNode() : base(new INode<AstNode>[0])
        {
        }

        protected ElementNode(IEnumerable<INode<AstNode>> childNodes)
            : base(childNodes)
        {
        }
    }
}