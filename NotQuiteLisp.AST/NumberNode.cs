namespace NotQuiteLisp.AST
{
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

        public override AstNode Clone()
        {
            return new NumberNode(_numberText);
        }
    }
}