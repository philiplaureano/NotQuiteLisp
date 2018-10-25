namespace NotQuiteLisp.AST
{
    using NotQuiteLisp.AST.Interfaces;

    public class OperatorNode : SymbolNode
    {
        private readonly string _operator;

        public OperatorNode(string @operator) : base(@operator)
        {
            _operator = @operator;
        }

        public string Operator
        {
            get
            {
                return _operator;
            }
        }

        public override INode<AstNode> Clone()
        {
            return new OperatorNode(_operator);
        }
    }
}