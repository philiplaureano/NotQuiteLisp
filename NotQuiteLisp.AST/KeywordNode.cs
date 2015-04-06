namespace NotQuiteLisp.AST
{
    public class KeywordNode : AtomNode
    {
        private readonly string _keyword;

        public KeywordNode(string keyword)
        {
            this._keyword = keyword;
        }

        public string Keyword
        {
            get
            {
                return this._keyword;
            }
        }
    }
}