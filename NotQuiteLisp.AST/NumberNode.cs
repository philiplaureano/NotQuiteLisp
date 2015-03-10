namespace NotQuiteLispParser
{
    public class NumberNode : AstNode
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
    }
}