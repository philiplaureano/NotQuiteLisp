using System.Text;

namespace NotQuiteLisp.Core
{
    using System;
    using System.Collections.Generic;

    using NotQuiteLisp.AST;
    using NotQuiteLisp.AST.Interfaces;

    public class BoundScope<TItem> : Node<IScope<TItem>>, IBoundScope<TItem>
        where TItem : ISymbol
    {
        private readonly IScope<TItem> _scope;
        private readonly INode<AstNode> _node;

        public BoundScope(IScope<TItem> scope, INode<AstNode> node)
        {
            if (scope == null)
                throw new ArgumentNullException("scope");

            if (node == null)
                throw new ArgumentNullException("node");

            _scope = scope;
            _node = node;
        }

        public BoundScope(IScope<TItem> scope, INode<AstNode> node, IEnumerable<INode<IScope<TItem>>> childNodes)
            : base(childNodes)
        {
            if (scope == null)
                throw new ArgumentNullException("scope");

            if (node == null)
                throw new ArgumentNullException("node");

            this._scope = scope;
            this._node = node;
        }

        public INode<AstNode> Node
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
                var namedScope = _scope as INameable;
                if (namedScope == null)
                    return null;

                var localName = namedScope.Name;

                var parentScopeNames = new Stack<string>();
                var parentScope = _scope.OuterScope;
                while (parentScope != null)
                {
                    var currentNamedScope = parentScope as INameable;

                    var parentName = currentNamedScope != null ? currentNamedScope.Name : "{anonymous}";
                    parentScopeNames.Push(parentName);

                    parentScope = parentScope.OuterScope;
                }

                var builder = new StringBuilder();
                while (parentScopeNames.Count > 0)
                {
                    builder.Append(parentScopeNames.Pop());
                    builder.Append("/");
                }

                builder.Append(localName);
                return builder.ToString();
            }
        }

        public IScope<TItem> OuterScope
        {
            get
            {
                return _scope.OuterScope;
            }
        }

        public IScope<TItem> TargetScope
        {
            get
            {
                return _scope;
            }
        }
        public TItem Resolve(string name)
        {
            return _scope.Resolve(name);
        }

        public void Define(TItem symbol)
        {
            _scope.Define(symbol);
        }

        public IEnumerable<KeyValuePair<string, TItem>> Symbols
        {
            get
            {
                return _scope.Symbols;
            }
        }

        public override INode<IScope<TItem>> Clone()
        {
            var clone = new BoundScope<TItem>(_scope, _node, this.Children);

            return clone;
        }
    }
}