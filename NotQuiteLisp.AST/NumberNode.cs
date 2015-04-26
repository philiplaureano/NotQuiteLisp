namespace NotQuiteLisp.AST
{
    using NotQuiteLisp.AST.Interfaces;

    public class NumberNode : AtomNode
    {
        private readonly string _numberText;

        public NumberNode(string numberText)
        {
            this._numberText = numberText;
        }

        public string Number
        {
            get
            {
                return this._numberText;
            }
        }

        public override INode<AstNode> Clone()
        {
            return new NumberNode(_numberText);
        }
    }
}