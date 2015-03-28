namespace NotQuiteLisp.AST
{
    using System.Collections.Generic;

    public class SetNode : ListNode
    {
        public SetNode(params AstNode[] childNodes)
            : base(childNodes)
        {
        }

        public SetNode(IEnumerable<AstNode> childNodes)
            : base(childNodes)
        {
        }
    }
}