using System.Linq;

namespace NotQuiteLisp.AST
{
    using System.Collections.Generic;

    using NotQuiteLisp.AST.Interfaces;

    public class ListNode : ElementNode
    {
        public ListNode(params INode<AstNode>[] childNodes)
            : this((IEnumerable<INode<AstNode>>)childNodes)
        {
        }
        public ListNode(IEnumerable<INode<AstNode>> childNodes)
            : base(childNodes)
        {
        }

        public override INode<AstNode> Clone()
        {
            var clonedChildren = Children.Select(c => c.Clone()).ToArray();
            return new ListNode(clonedChildren);
        }
    }
}