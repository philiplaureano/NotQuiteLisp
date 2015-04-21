namespace NotQuiteLisp.AST
{
    public class VariableDeclarationNode : SymbolNode
    {
        private readonly AstNode _value;

        public VariableDeclarationNode(string variableName, AstNode value) : base(variableName)
        {
            _value = value;
        }

        public AstNode Value
        {
            get { return _value; }
        }
    }
}