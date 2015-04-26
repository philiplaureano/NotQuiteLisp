namespace NotQuiteLisp.AST
{
    using NotQuiteLisp.AST.Interfaces;

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

        public override INode<AstNode> Clone()
        {
            return new SymbolNode(_symbol);
        }
    }
}