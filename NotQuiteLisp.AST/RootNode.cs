using System.Linq;
using NotQuiteLisp.AST.Interfaces;

namespace NotQuiteLisp.AST
{
    using System.Collections.Generic;

    public sealed class RootNode : AstNode
    {
        public RootNode() : base(new INode<AstNode>[0])
        {
        }

        public RootNode(IEnumerable<INode<AstNode>> childNodes)
            : base(childNodes)
        {
        }

        public override INode<AstNode> Clone()
        {
            var clonedChildren = Children.Select(c => c.Clone());
            return new RootNode(clonedChildren);
        }
    }
}