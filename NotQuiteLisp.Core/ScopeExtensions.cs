namespace NotQuiteLisp.Core
{
    using System;
    using System.Collections.Generic;

    using NotQuiteLisp.AST;
    using NotQuiteLisp.AST.Interfaces;

    public static class ScopeExtensions
    {
        public static IEnumerable<SymbolNode> UnresolvedSymbols(this IBoundScope scope)
        {
            var unresolvedSymbols = new List<SymbolNode>();

            var parentScope = scope;
            Action<int, INode<IScope>> addUnresolvedSymbol
                = (depth, node) =>
                    {
                        var boundScope = node as IBoundScope;
                        if (boundScope == null)
                            return;

                        var symbolNode = boundScope.Node as SymbolNode;
                        if (symbolNode == null) 
                            return;

                        if (parentScope.Resolve(symbolNode.Symbol) == null)
                            unresolvedSymbols.Add(symbolNode);
                    };

            scope.Descend(addUnresolvedSymbol);

            return unresolvedSymbols;
        }
    }
}