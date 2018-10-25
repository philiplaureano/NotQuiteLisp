namespace NotQuiteLisp.AST
{
    using NotQuiteLisp.AST.Interfaces;

    public class StringNode : AtomNode
    {
        private readonly string _text;

        public StringNode(string text)
        {
            _text = text;
        }

        public string Text
        {
            get
            {
                return _text;
            }
        }

        public override INode<AstNode> Clone()
        {
            return new StringNode(_text);
        }
    }
}