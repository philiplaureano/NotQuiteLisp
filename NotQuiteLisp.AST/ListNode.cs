namespace NotQuiteLispParser
{
    using System.Collections.Generic;

    public class ListNode : AstNode
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