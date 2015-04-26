namespace NotQuiteLisp.AST
{
    using NotQuiteLisp.AST.Interfaces;

    public sealed class FalseNode : BooleanNode
    {
        public override INode<AstNode> Clone()
        {
            return new FalseNode();
        }
    }
}