using System.Collections.Generic;

namespace NotQuiteLisp.AST
{
    public class VectorNode : ListNode
    {
        public VectorNode(params AstNode[] childNodes) : base(childNodes)
        {
        }

        public VectorNode(IEnumerable<AstNode> childNodes) : base(childNodes)
        {
        }
    }
}