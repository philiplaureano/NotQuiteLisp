using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NotQuiteLisp.AST.Interfaces;

    public class SymbolNodeScopeBuilder : ScopeBuilder<SymbolNode>
    {
        public SymbolNodeScopeBuilder(IScope<SymbolNode> rootScope) : base(rootScope)
        {
        }

        protected override void Define(ParameterDefinitionNode node, IScope<SymbolNode> parentScope)
        {
            parentScope.Define(node);
        }

        protected override void Define(MethodDefinitionNode node, IScope<SymbolNode> parentScope)
        {
            parentScope.Define(node);
        }

        protected override void Define(VariableDefinitionNode node, IScope<SymbolNode> parentScope)
        {
            parentScope.Define(node);
        }
    }

}