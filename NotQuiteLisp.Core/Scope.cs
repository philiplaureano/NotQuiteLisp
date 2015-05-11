using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NotQuiteLisp.AST;
using NotQuiteLisp.AST.Interfaces;

namespace NotQuiteLisp.Core
{
    public abstract class Scope<TItem> : IScope<TItem>
        where TItem : ISymbol
    {
        private readonly ConcurrentDictionary<string, TItem> _symbols =
            new ConcurrentDictionary<string, TItem>();

        private IScope<TItem> _outerScope;

        protected Scope(IScope<TItem> outerScope)
        {
            _outerScope = outerScope;
        }

        public abstract string Name { get; }

        public IScope<TItem> OuterScope
        {
            get { return _outerScope; }
            set { _outerScope = value; }
        }

        public virtual void Define(TItem symbol)
        {
            var symbolName = symbol.Symbol;
            if (_symbols.ContainsKey(symbolName))
                throw new InvalidOperationException(string.Format("The symbol '{0}' has already been defined in the current scope", symbolName));

            // Check the parent scope
            if (_outerScope != null && !ReferenceEquals(_outerScope.Resolve(symbolName), null))
                throw new InvalidOperationException(string.Format("The symbol '{0}' has already been defined in the parent scope", symbolName));

            _symbols[symbolName] = symbol;
        }

        public virtual TItem Resolve(string name)
        {
            if (_symbols.ContainsKey(name))
                return _symbols[name];

            if (_outerScope != null)
                return _outerScope.Resolve(name);

            return default(TItem);
        }

        public IEnumerable<KeyValuePair<string, TItem>> Symbols
        {
            get { return _symbols; }
        }

        public void Clear()
        {
            _symbols.Clear();
        }
    }
}