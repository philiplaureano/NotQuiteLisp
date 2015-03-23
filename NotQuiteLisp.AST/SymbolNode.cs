namespace NotQuiteLisp.AST
{
    public class SymbolNode : AstNode
    {
        private readonly string _symbol;

        public SymbolNode(string symbol)
        {
            this._symbol = symbol;
        }

        public string Symbol
        {
            get
            {
                return this._symbol;
            }
        }
    }
}