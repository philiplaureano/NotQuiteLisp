using System.Text;

namespace NotQuiteLisp.Core
{
    using System;
    using System.Collections.Generic;

    using NotQuiteLisp.AST;
    using NotQuiteLisp.AST.Interfaces;

    public class BoundScope : Node<IScope>, IBoundScope
    {
        private readonly IScope _scope;
        private readonly INode<AstNode> _node;

        public BoundScope(IScope scope, INode<AstNode> node)
        {
            if (scope == null)
                throw new ArgumentNullException("scope");

            if (node == null)
                throw new ArgumentNullException("node");

            _scope = scope;
            _node = node;
        }

        public BoundScope(IScope scope, INode<AstNode> node, IEnumerable<INode<IScope>> childNodes)
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
                var namedScope = _scope as INamedScope;
                if (namedScope == null)
                    return null;

                var localName = namedScope.Name;

                var parentScopeNames = new Stack<string>();
                var parentScope = _scope.OuterScope;
                while (parentScope != null)
                {
                    var currentNamedScope = parentScope as INamedScope;

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

        public override INode<IScope> Clone()
        {
            var clone = new BoundScope(_scope, _node, this.Children);

            return clone;
        }
    }
}