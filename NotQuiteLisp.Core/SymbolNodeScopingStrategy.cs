namespace NotQuiteLisp.Core
{
    using NotQuiteLisp.AST;
    using NotQuiteLisp.AST.Interfaces;

    public class SymbolNodeScopingStrategy : IScopingStrategy<SymbolNode>
    {
        public void Define(ParameterDefinitionNode node, IScope<SymbolNode> parentScope)
        {
            parentScope.Define(node.Symbol, node);
        }

        public void Define(MethodDefinitionNode node, IScope<SymbolNode> parentScope)
        {
            parentScope.Define(node.Symbol, node);
        }

        public void Define(VariableDefinitionNode node, IScope<SymbolNode> parentScope)
        {
            parentScope.Define(node.Symbol, node);
        }
    }
}