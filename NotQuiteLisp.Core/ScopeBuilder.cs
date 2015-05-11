﻿using System;
using System.Linq;
using NotQuiteLisp.AST;
using NotQuiteLisp.AST.Interfaces;
using NotQuiteLisp.Visitors;

namespace NotQuiteLisp.Core
{
    public abstract class ScopeBuilder<TItem> : IVisitor<AstNode, IBoundScope<TItem>>
        where TItem : ISymbol
    {
        private readonly IScope<TItem> _rootScope;

        protected ScopeBuilder(IScope<TItem> rootScope)
        {
            this._rootScope = rootScope;
        }

        public IBoundScope<TItem> Visit(AstNode subject)
        {
            try
            {
                var boundScope = (IBoundScope<TItem>)this.Invoke("GetScope", subject);
                return boundScope;
            }
            catch (MethodNotFoundException)
            {
                // Ignore the error and bind the node to the root scope
            }

            return new BoundScope<TItem>(_rootScope, subject);
        }

        public IBoundScope<TItem> GetScope(SymbolNode node, IScope<TItem> parentScope)
        {
            // Method definitions don't count as scope references
            if (node is MethodDefinitionNode)
                return GetScope(node as MethodDefinitionNode, parentScope);

            return new BoundScope<TItem>(parentScope, node);
        }

        public IBoundScope<TItem> GetScope(MethodDefinitionNode node, IScope<TItem> parentScope)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            var methodName = node.MethodName;
            var methodScope = new MethodScope<TItem>(methodName, parentScope);
            var boundScope = new BoundScope<TItem>(methodScope, node);

            // Define the method itself
            Define(node, parentScope);

            // Bind the parameters
            foreach (var parameter in node.Parameters)
            {
                Define(parameter, methodScope);
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
                Define(declaration, methodScope);
            }

            return boundScope;
        }

        public IBoundScope<TItem> GetScope(ListNode node, IScope<TItem> parentScope)
        {
            var childScopes = node.Children
                .Select(child => (IBoundScope<TItem>)this.Invoke("GetScope", child, parentScope))
                .ToList();

            return new BoundScope<TItem>(parentScope, node, childScopes);
        }

        public IBoundScope<TItem> GetScope(RootNode node)
        {
            var childScopes = node.Children
                .Select(child => (IBoundScope<TItem>)this.Invoke("GetScope", child, this._rootScope))
                .ToList();

            return new BoundScope<TItem>(_rootScope, node, childScopes);
        }

        protected abstract void Define(ParameterDefinitionNode node, IScope<TItem> parentScope);
        protected abstract void Define(MethodDefinitionNode node, IScope<TItem> parentScope);
        protected abstract void Define(VariableDefinitionNode node, IScope<TItem> parentScope);
    }
}