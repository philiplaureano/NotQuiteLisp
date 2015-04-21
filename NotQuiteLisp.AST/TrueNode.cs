namespace NotQuiteLisp.AST
{
    public sealed class TrueNode : BooleanNode
    {
        public override AstNode Clone()
        {
            return new TrueNode();
        }
    }
}