namespace NotQuiteLispParser
{
    public class OperatorNode : AstNode
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
    }
}