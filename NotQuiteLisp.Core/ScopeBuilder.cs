using NotQuiteLisp.AST;
using NotQuiteLisp.Visitors;

namespace NotQuiteLisp.Core
{
    using System;
    using System.Linq.Expressions;

    using NotQuiteLisp.AST.Interfaces;

    public class ScopeBuilder : IVisitor<AstNode, IScope>
    {
        private readonly IScope _rootScope;

        public ScopeBuilder(IScope rootScope)
        {
            this._rootScope = rootScope;
        }

        public IScope Visit(AstNode subject)
        {
            return (IScope)this.Invoke("GetScope", subject);
        }

        public IScope GetScope(SymbolNode node, IScope parentScope)
        {
            parentScope.Define(node);

            return new BoundScope(parentScope, node);
        }

        public IScope GetScope(MethodDefinitionNode node, IScope parentScope)
        {
            var methodScope = new AnonymousScope(parentScope);
            var boundScope = new BoundScope(methodScope, node);

            // Bind the parameters
            foreach (var parameter in node.Parameters)
            {
                methodScope.Define(parameter);
            }

            return boundScope;
        }

        public IScope GetScope(ListNode node, IScope parentScope)
        {
            foreach (var child in node.Children)
            {
                this.Invoke("GetScope", child, _rootScope);
            }

            return new BoundScope(parentScope, node);
        }

        public IScope GetScope(RootNode node)
        {
            foreach (var child in node.Children)
            {
                this.Invoke("GetScope", child, _rootScope);
            }

            return new BoundScope(_rootScope, node);
        }
    }
}