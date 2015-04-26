namespace NotQuiteLisp.Core
{
    using System;
    using System.Collections.Generic;

    using NotQuiteLisp.AST;
    using NotQuiteLisp.AST.Interfaces;

    public interface IBoundScope : INode<IScope>, IScope
    {
        
    }
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

        public override INode<IScope> Clone()
        {            
            var clone = new BoundScope(_scope, _node, this.Children);

            return clone;
        }
    }
}