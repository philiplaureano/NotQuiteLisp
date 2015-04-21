namespace NotQuiteLisp.AST
{
    public sealed class NilNode : AtomNode
    {
        public override AstNode Clone()
        {
            return new NilNode();
        }
    }
}