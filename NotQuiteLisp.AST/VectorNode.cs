using System.Collections.Generic;

namespace NotQuiteLisp.AST
{
    using NotQuiteLisp.AST.Interfaces;

    public class VectorNode : ListNode
    {
        public VectorNode(params INode<AstNode>[] childNodes)
            : base(childNodes)
        {
        }

        public VectorNode(IEnumerable<INode<AstNode>> childNodes)
            : base(childNodes)
        {
        }
        public override INode<AstNode> Clone()
        {
            return new VectorNode(Children);
        }
    }
}