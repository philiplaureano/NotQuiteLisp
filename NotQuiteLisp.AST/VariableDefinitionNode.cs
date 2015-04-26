namespace NotQuiteLisp.AST
{
    using NotQuiteLisp.AST.Interfaces;

    public class VariableDefinitionNode : SymbolNode
    {
        private readonly INode<AstNode> _value;

        public VariableDefinitionNode(string variableName, INode<AstNode> value) : base(variableName)
        {
            _value = value;
        }

        public INode<AstNode> Value
        {
            get { return _value; }
        }

        public override INode<AstNode> Clone()
        {
            return new VariableDefinitionNode(Symbol, Value);
        }
    }
}