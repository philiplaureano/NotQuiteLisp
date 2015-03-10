namespace NotQuiteLispParser
{
    public class PairNode : AstNode
    {
        public PairNode(AstNode firstNode, AstNode secondNode)
            : base(new[] { firstNode, secondNode })
        {
        }
    }
}