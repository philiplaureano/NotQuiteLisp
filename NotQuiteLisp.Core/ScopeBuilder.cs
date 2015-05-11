using NotQuiteLisp.AST;
using NotQuiteLisp.Visitors;

namespace NotQuiteLisp.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NotQuiteLisp.AST.Interfaces;

    public class ScopeBuilder : IVisitor<AstNode, IBoundScope<SymbolNode>>
    {
        private readonly IScope<SymbolNode> _rootScope;

        public ScopeBuilder(IScope<SymbolNode> rootScope)
        {
            this._rootScope = rootScope;
        }

        public IBoundScope<SymbolNode> Visit(AstNode subject)
        {
            try
            {
                var boundScope = (IBoundScope<SymbolNode>)this.Invoke("GetScope", subject);
                return boundScope;
            }
            catch (MethodNotFoundException)
            {                
                // Ignore the error and bind the node to the root scope
            }
            
            return new BoundScope<SymbolNode>(_rootScope, subject);
        }

        public IBoundScope<SymbolNode> GetScope(SymbolNode node, IScope<SymbolNode> parentScope)
        {
            // Method definitions don't count as scope references
            if (node is MethodDefinitionNode) 
                return GetScope(node as MethodDefinitionNode, parentScope);

            return new BoundScope<SymbolNode>(parentScope, node);
        }

        public IBoundScope<SymbolNode> GetScope(MethodDefinitionNode node, IScope<SymbolNode> parentScope)
        {
            if (node == null) 
                throw new ArgumentNullException("node");

            var methodName = node.MethodName;
            var methodScope = new MethodScope<SymbolNode>(methodName, parentScope);
            var boundScope = new BoundScope<SymbolNode>(methodScope, node);

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

        public IBoundScope<SymbolNode> GetScope(ListNode node, IScope<SymbolNode> parentScope)
        {
            var childScopes = node.Children
                .Select(child => (IBoundScope<SymbolNode>)this.Invoke("GetScope", child, parentScope))
                .ToList();

            return new BoundScope<SymbolNode>(parentScope, node, childScopes);
        }

        public IBoundScope<SymbolNode> GetScope(RootNode node)
        {
            var childScopes = node.Children
                .Select(child => (IBoundScope<SymbolNode>)this.Invoke("GetScope", child, this._rootScope))
                .ToList();

            return new BoundScope<SymbolNode>(_rootScope, node, childScopes);
        }
    }
}