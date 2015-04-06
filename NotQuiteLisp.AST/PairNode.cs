namespace NotQuiteLisp.AST
{
    public class PairNode : AstNode
    {
        public PairNode(AstNode key, AstNode valueNode)
            : base(new[] { key, valueNode })
        {
        }
    }
}