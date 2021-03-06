namespace NotQuiteLisp.AST
{
    using NotQuiteLisp.AST.Interfaces;

    public class OperatorNode : SymbolNode
    {
        private readonly string _operator;

        public OperatorNode(string @operator) : base(@operator)
        {
            this._operator = @operator;
        }

        public string Operator
        {
            get
            {
                return this._operator;
            }
        }

        public override INode<AstNode> Clone()
        {
            return new OperatorNode(_operator);
        }
    }
}