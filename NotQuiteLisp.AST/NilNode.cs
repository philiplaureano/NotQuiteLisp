namespace NotQuiteLisp.AST
{
    using NotQuiteLisp.AST.Interfaces;

    public sealed class NilNode : AtomNode
    {
        public override INode<AstNode> Clone()
        {
            return new NilNode();
        }
    }
}