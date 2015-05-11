namespace NotQuiteLisp.Core
{
    using NotQuiteLisp.AST;
    using NotQuiteLisp.AST.Interfaces;

    public interface IScopingStrategy<TItem>
        where TItem : ISymbol
    {
        void Define(ParameterDefinitionNode node, IScope<TItem> parentScope);
        void Define(MethodDefinitionNode node, IScope<TItem> parentScope);
        void Define(VariableDefinitionNode node, IScope<TItem> parentScope);
    }
}