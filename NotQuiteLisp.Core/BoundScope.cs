namespace NotQuiteLisp.Core
{
    using System;
    using System.Collections.Generic;

    using NotQuiteLisp.AST;
    using NotQuiteLisp.AST.Interfaces;

    public class BoundScope : IScope
    {
        private readonly IScope _scope;
        private readonly AstNode _node;

        public BoundScope(IScope scope, AstNode node)
        {
            if (scope == null)
                throw new ArgumentNullException("scope");

            if (node == null)
                throw new ArgumentNullException("node");

            _scope = scope;
            _node = node;
        }

        public AstNode Node
        {
            get
            {
                return this._node;
            }
        }

        public string Name
        {
            get
            {
                return _scope.Name;
            }
        }

        public IScope OuterScope
        {
            get
            {
                return _scope.OuterScope;
            }
        }

        public IScope TargetScope
        {
            get
            {
                return _scope;
            }
        }
        public SymbolNode Resolve(string name)
        {
            return _scope.Resolve(name);
        }

        public void Define(SymbolNode symbol)
        {
            _scope.Define(symbol);
        }

        public IEnumerable<KeyValuePair<string, SymbolNode>> Symbols
        {
            get
            {
                return _scope.Symbols;
            }
        }
    }
}