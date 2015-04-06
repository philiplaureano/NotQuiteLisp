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
    }
}