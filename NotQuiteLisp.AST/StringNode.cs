namespace NotQuiteLispParser
{
    public class StringNode : AstNode
    {
        private readonly string _text;

        public StringNode(string text)
        {
            this._text = text;
        }

        public string Text
        {
            get
            {
                return this._text;
            }
        }
    }
}