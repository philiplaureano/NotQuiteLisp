namespace NotQuiteLisp.AST
{
    public class StringNode : AtomNode
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