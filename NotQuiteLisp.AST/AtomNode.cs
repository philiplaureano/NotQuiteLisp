using NotQuiteLisp.AST.Interfaces;

namespace NotQuiteLisp.AST
{
    using System.Collections.Generic;

    public abstract class AtomNode : ElementNode
    {
        protected AtomNode()
        {
        }

        protected AtomNode(IEnumerable<INode<AstNode>> childNodes)
            : base(childNodes)
        {
        }
    }
}