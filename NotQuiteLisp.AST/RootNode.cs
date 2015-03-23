namespace NotQuiteLisp.AST
{
    using System.Collections.Generic;

    public sealed class RootNode : AstNode
    {
        public RootNode()
        {
        }

        public RootNode(IEnumerable<AstNode> childNodes)
            : base(childNodes)
        {
        }
    }
}