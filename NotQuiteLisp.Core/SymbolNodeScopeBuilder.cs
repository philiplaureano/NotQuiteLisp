using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NotQuiteLisp.AST.Interfaces;

    public class SymbolNodeScopeBuilder : ScopeBuilder<SymbolNode>
    {
        public SymbolNodeScopeBuilder(IScope<SymbolNode> rootScope) : base(rootScope, new SymbolNodeScopingStrategy())
        {
        }        
    }

}