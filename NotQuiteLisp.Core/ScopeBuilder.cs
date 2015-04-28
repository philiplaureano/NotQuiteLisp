using NotQuiteLisp.AST;
using NotQuiteLisp.Visitors;

namespace NotQuiteLisp.Core
{
    using System;
    using System.Collections.Generic;
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
            try
            {
                var boundScope = (IBoundScope)this.Invoke("GetScope", subject);
                return boundScope;
            }
            catch (MethodNotFoundException)
            {                
                // Ignore the error and bind the node to the root scope
            }
            
            return new BoundScope(_rootScope, subject);
        }

        public IBoundScope GetScope(SymbolNode node, IScope parentScope)
        {
            return new BoundScope(parentScope, node);
        }

        public IBoundScope GetScope(MethodDefinitionNode node, IScope parentScope)
        {
            if (node == null) 
                throw new ArgumentNullException("node");

            var methodName = node.MethodName;
            var methodScope = new MethodScope(methodName, parentScope);
            var boundScope = new BoundScope(methodScope, node);

            // Define the method itself
            parentScope.Define(node);

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
                .OfType<VariableDefinitionNode>()
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
            var childScopes = node.Children
                .Select(child => (IBoundScope)this.Invoke("GetScope", child, parentScope))
                .ToList();

            return new BoundScope(parentScope, node, childScopes);
        }

        public IBoundScope GetScope(RootNode node)
        {
            var childScopes = node.Children
                .Select(child => (IBoundScope)this.Invoke("GetScope", child, this._rootScope))
                .ToList();

            return new BoundScope(_rootScope, node, childScopes);
        }
    }
}