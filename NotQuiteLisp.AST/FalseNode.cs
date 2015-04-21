namespace NotQuiteLisp.AST
{
    public sealed class FalseNode : BooleanNode
    {
        public override AstNode Clone()
        {
            return new FalseNode();
        }
    }
}