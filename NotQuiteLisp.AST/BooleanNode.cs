namespace NotQuiteLisp.AST
{
    public abstract class BooleanNode : AtomNode
    {
        public static readonly BooleanNode True = new TrueNode();
        public static readonly BooleanNode False = new FalseNode();
    }
}