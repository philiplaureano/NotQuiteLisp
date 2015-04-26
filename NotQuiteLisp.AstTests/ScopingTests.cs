using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotQuiteLisp.AST;
using NotQuiteLisp.AST.Interfaces;
using NotQuiteLisp.Core;
using Shouldly;

namespace NotQuiteLisp.AstTests
{
    using FakeItEasy;

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
        public void Should_return_globalscope_for_root_node()
        {
            var rootNode = new RootNode();

            var builder = new ScopeBuilder(new GlobalScope());
            var scope = builder.Visit(rootNode) as BoundScope;
            scope.ShouldBeOfType<BoundScope>();
            scope.ShouldNotBe(null);

            scope.Node.ShouldBe(rootNode);
            scope.TargetScope.ShouldBeOfType<GlobalScope>();
        }

        [TestMethod]
        public void Should_scope_nested_symbols()
        {
            var greatGrandChild = new SymbolNode("ghi");
            var grandChildren = new AstNode[] { new SymbolNode("abc"), new SymbolNode("def"), new ListNode(greatGrandChild) };

            var children = new AstNode[] { new ListNode(grandChildren), };
            var fakeScope = A.Fake<IScope>();

            var rootNode = new RootNode(children);

            var builder = new ScopeBuilder(fakeScope);
            builder.Visit(rootNode).ShouldNotBe(null);

            A.CallTo(() => fakeScope.Define((SymbolNode)grandChildren[0])).MustHaveHappened();
            A.CallTo(() => fakeScope.Define((SymbolNode)grandChildren[1])).MustHaveHappened();
            A.CallTo(() => fakeScope.Define((SymbolNode)greatGrandChild)).MustHaveHappened();
        }

        [TestMethod]
        public void Should_scope_symbols()
        {
            var children = new AstNode[] { new SymbolNode("abc"), new SymbolNode("def") };
            var fakeScope = A.Fake<IScope>();

            var rootNode = new RootNode(children);

            var builder = new ScopeBuilder(fakeScope);
            builder.Visit(rootNode).ShouldNotBe(null);

            A.CallTo(() => fakeScope.Define((SymbolNode)children[0])).MustHaveHappened();
            A.CallTo(() => fakeScope.Define((SymbolNode)children[1])).MustHaveHappened();
        }

        [TestMethod]
        public void Should_scope_parameters()
        {
            var body = new ListNode();
            var methodDefinitionNode = new MethodDefinitionNode("sayMessage", new ParameterDefinitionNode[]{new ParameterDefinitionNode("message"), }, body);

            var globalScope = new GlobalScope();
            
            var builder = new ScopeBuilder(globalScope);
            var resultScope = builder.GetScope(methodDefinitionNode, globalScope);
            resultScope.Resolve("message").ShouldNotBe(null);
            resultScope.Resolve("message").ShouldBeOfType<ParameterDefinitionNode>();
        }
    }
}