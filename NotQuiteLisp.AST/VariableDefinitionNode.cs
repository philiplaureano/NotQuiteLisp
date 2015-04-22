namespace NotQuiteLisp.AST
{
    public class VariableDefinitionNode : SymbolNode
    {
        private readonly AstNode _value;

        public VariableDefinitionNode(string variableName, AstNode value) : base(variableName)
        {
            _value = value;
        }

        public AstNode Value
        {
            get { return _value; }
        }

        public override AstNode Clone()
        {
            return new VariableDefinitionNode(Symbol, Value);
        }
    }
}