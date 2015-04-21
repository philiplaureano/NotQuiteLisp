namespace NotQuiteLisp.AST
{
    public class SymbolNode : AtomNode
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

        public override AstNode Clone()
        {
            return new SymbolNode(_symbol);
        }
    }
}