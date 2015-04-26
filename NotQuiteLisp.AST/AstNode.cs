namespace NotQuiteLisp.AST
{
    using System.Collections.Generic;

    using NotQuiteLisp.AST.Interfaces;

    public abstract class AstNode : Node<AstNode>
    {
        protected AstNode(IEnumerable<INode<AstNode>> childNodes)
            : base(childNodes)
        {
        }
    }
}