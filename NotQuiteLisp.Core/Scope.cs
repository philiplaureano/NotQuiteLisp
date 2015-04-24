using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NotQuiteLisp.AST;
using NotQuiteLisp.AST.Interfaces;

namespace NotQuiteLisp.Core
{
    public abstract class Scope : IScope
    {
        private readonly ConcurrentDictionary<string, SymbolNode> _symbols =
            new ConcurrentDictionary<string, SymbolNode>();

        private IScope _outerScope;

        protected Scope(IScope outerScope)
        {
            _outerScope = outerScope;
        }

        public abstract string Name { get; }

        public IScope OuterScope
        {
            get { return _outerScope; }
            set { _outerScope = value; }
        }

        public virtual void Define(SymbolNode symbol)
        {
            var symbolName = symbol.Symbol;
            if (_symbols.ContainsKey(symbolName))
                throw new InvalidOperationException(string.Format("The symbol '{0}' has already been defined in the current scope", symbolName));

            // Check the parent scope
            if (_outerScope != null && _outerScope.Resolve(symbolName) != null)
                throw new InvalidOperationException(string.Format("The symbol '{0}' has already been defined in the parent scope", symbolName));

            _symbols[symbolName] = symbol;
        }

        public virtual SymbolNode Resolve(string name)
        {
            if (_symbols.ContainsKey(name))
                return _symbols[name];

            if (_outerScope != null)
                return _outerScope.Resolve(name);

            return null;
        }

        public IEnumerable<KeyValuePair<string, SymbolNode>> Symbols
        {
            get { return _symbols; }
        }

        public void Clear()
        {
            _symbols.Clear();
        }
    }
}