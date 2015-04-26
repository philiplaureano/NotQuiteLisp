using NotQuiteLisp.AST;
using NotQuiteLisp.Visitors;

namespace NotQuiteLisp.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using NotQuiteLisp.AST.Interfaces;

    public class ScopeBuilder : IVisitor<AstNode, IBoundScope>
    {
        private readonly IScope _rootScope;

        public ScopeBuilder(IScope rootScope)
        {
            this._rootScope = rootScope;
        }

        public IBoundScope Visit(AstNode subject)
        {
            return (IBoundScope)this.Invoke("GetScope", subject);
        }

        public IBoundScope GetScope(SymbolNode node, IScope parentScope)
        {
            parentScope.Define(node);

            return new BoundScope(parentScope, node);
        }

        public IBoundScope GetScope(MethodDefinitionNode node, IScope parentScope)
        {
            var methodScope = new AnonymousScope(parentScope);
            var boundScope = new BoundScope(methodScope, node);

            // Bind the parameters
            foreach (var parameter in node.Parameters)
            {
                methodScope.Define(parameter);
            }

            // Bind the variables
            var body = node.MethodBody;
            if (body == null) 
                return boundScope;

            var variableDeclarations = body.Children
                .Select(n => n as VariableDefinitionNode)
                .Where(n => n != null)
                .ToArray();

            foreach (var declaration in variableDeclarations)
            {
                methodScope.Define(declaration);
            }

            return boundScope;
        }

        public IBoundScope GetScope(ListNode node, IScope parentScope)
        {
            foreach (var child in node.Children)
            {
                this.Invoke("GetScope", child, _rootScope);
            }

            return new BoundScope(parentScope, node);
        }

        public IBoundScope GetScope(RootNode node)
        {
            foreach (var child in node.Children)
            {
                this.Invoke("GetScope", child, _rootScope);
            }

            return new BoundScope(_rootScope, node);
        }
    }
}