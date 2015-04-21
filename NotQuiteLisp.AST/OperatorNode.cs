namespace NotQuiteLisp.AST
{
    public class OperatorNode : AtomNode
    {
        private readonly string _operator;

        public OperatorNode(string @operator)
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

        public override AstNode Clone()
        {
            return new OperatorNode(_operator);
        }
    }
}