namespace NotQuiteLisp.Core
{
    using System;
    using System.Collections.Generic;

    using NotQuiteLisp.AST;
    using NotQuiteLisp.AST.Interfaces;

    public static class ScopeExtensions
    {
        public static IEnumerable<IBoundScope<TItem>> UnresolvedSymbols<TItem>(this IBoundScope<TItem> scope)
            where TItem : class, ISymbol
        {
            var unresolvedSymbols = new List<IBoundScope<TItem>>();

            var parentScope = scope;
            Action<int, int, INode<IScope<TItem>>> addUnresolvedSymbol
                = (depth, childIndex, node) =>
                    {
                        var boundScope = node as IBoundScope<TItem>;
                        if (boundScope == null)
                            return;

                        var symbolNode = boundScope.Node as SymbolNode;
                        if (symbolNode == null) 
                            return;

                        if (parentScope.Resolve(symbolNode.Symbol) == null)
                            unresolvedSymbols.Add(boundScope);
                    };

            scope.Descend(addUnresolvedSymbol);

            return unresolvedSymbols;
        }
    }
}