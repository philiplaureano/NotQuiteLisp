namespace NotQuiteLisp.AST
{
    using NotQuiteLisp.AST.Interfaces;

    public sealed class TrueNode : BooleanNode
    {
        public override INode<AstNode> Clone()
        {
            return new TrueNode();
        }
    }
}