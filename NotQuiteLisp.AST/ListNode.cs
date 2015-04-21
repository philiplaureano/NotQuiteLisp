using System.Linq;

namespace NotQuiteLisp.AST
{
    using System.Collections.Generic;

    public class ListNode : ElementNode
    {
        public ListNode(params AstNode[] childNodes)
            : this((IEnumerable<AstNode>)childNodes)
        {
        }
        public ListNode(IEnumerable<AstNode> childNodes)
            : base(childNodes)
        {
        }

        public override AstNode Clone()
        {
            var clonedChildren = Children.Select(c => c.Clone()).ToArray();
            return new ListNode(clonedChildren);
        }
    }
}