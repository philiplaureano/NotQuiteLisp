using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotQuiteLisp.AST;
using NotQuiteLisp.AST.Interfaces;
using NotQuiteLisp.Core;
using Shouldly;

namespace NotQuiteLisp.AstTests
{
    [TestClass]
    public class ScopingTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "The symbol 'foo' has already been defined in the current scope")]
        public void Should_throw_exception_when_defining_already_defined_symbol()
        {
            var symbol = new SymbolNode("foo");
            var scope = new AnonymousScope(null);

            // Defining the same scope twice should cause an error
            scope.Define(symbol);
            scope.Define(symbol);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "The symbol 'foo' has already been defined in the parent scope")]
        public void Should_throw_exception_when_defining_already_defined_symbol_from_parent_scope()
        {
            var parentScope = new AnonymousScope(null);
            var symbol = new SymbolNode("foo");
            var scope = new AnonymousScope(parentScope);

            // Defining the same scope twice should cause an error
            parentScope.Define(symbol);
            scope.Define(symbol);
        }

        [TestMethod]
        public void Should_resolve_defined_symbol()
        {
            var symbol = new SymbolNode("foo");
            var scope = new AnonymousScope(null);

            scope.Define(symbol);

            scope.Resolve("foo").ShouldBe(symbol);
        }

        [TestMethod]
        public void Should_resolve_defined_symbol_in_parent_scope()
        {
            var symbol = new SymbolNode("foo");
            var parentScope = new AnonymousScope(null);
            var scope = new AnonymousScope(parentScope);

            parentScope.Define(symbol);

            scope.Resolve("foo").ShouldBe(symbol);
        }

        [TestMethod]
        public void Should_wrap_root_node_with_global_scope()
        {
            var rootNode = new RootNode();

            var builder = new ScopeBuilder();
            var outputNode = builder.Visit(rootNode);

            outputNode.ShouldBeAssignableTo<IScopeReference>();

            var scopeRef = outputNode as IScopeReference;
            scopeRef.ShouldNotBe(null);
            scopeRef.TargetScope.ShouldNotBe(null);
            scopeRef.TargetScope.ShouldBeOfType<GlobalScope>();
        }

        [TestMethod]
        public void Should_wrap_all_symbols_with_anonymous_scopes()
        {
            var children = new AstNode[] { new SymbolNode("abc"), new SymbolNode("def") };
            var rootNode = new RootNode(children);

            var builder = new ScopeBuilder();
            var outputNode = builder.Visit(rootNode);
            outputNode.Descendants().Count(d => d is ScopedNode).ShouldBe(2);
        }
    }
}