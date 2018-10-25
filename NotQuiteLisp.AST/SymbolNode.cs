namespace NotQuiteLisp.AST
{
    using NotQuiteLisp.AST.Interfaces;

    public class SymbolNode : AtomNode, ISymbol
    {
        private readonly string _symbol;

        public SymbolNode(string symbol)
        {
            _symbol = symbol;
        }

        public string Symbol
        {
            get
            {
                return _symbol;
            }
        }

        public override INode<AstNode> Clone()
        {
            return new SymbolNode(_symbol);
        }
    }
}