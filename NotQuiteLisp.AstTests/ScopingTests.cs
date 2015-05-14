using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotQuiteLisp.AST;
using NotQuiteLisp.AST.Interfaces;
using NotQuiteLisp.Core;
using Shouldly;

namespace NotQuiteLisp.AstTests
{
    using System.Collections.Generic;

    using FakeItEasy;

    [TestClass]
    public class ScopingTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "The symbol 'foo' has already been defined in the current scope")]
        public void Should_throw_exception_when_defining_already_defined_symbol()
        {
            var symbol = new SymbolNode("foo");
            var scope = new AnonymousScope<SymbolNode>(null);

            // Defining the same scope twice should cause an error
            scope.Define(symbol.Symbol, symbol);
            scope.Define(symbol.Symbol, symbol);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "The symbol 'foo' has already been defined in the parent scope")]
        public void Should_throw_exception_when_defining_already_defined_symbol_from_parent_scope()
        {
            var parentScope = new AnonymousScope<SymbolNode>(null);
            var symbol = new SymbolNode("foo");
            var scope = new AnonymousScope<SymbolNode>(parentScope);

            // Defining the same scope twice should cause an error
            parentScope.Define(symbol.Symbol, symbol);
            scope.Define(symbol.Symbol, symbol);
        }

        [TestMethod]
        public void Should_return_entire_bound_scope_name_as_path_to_root_scope()
        {
            var fakeNode = A.Fake<INode<AstNode>>();
            var fakeScope = A.Fake<INamedScope<SymbolNode>>();
            A.CallTo(() => fakeScope.Name).Returns("FakeScope");

            var globalScope = new GlobalScope<SymbolNode>();
            A.CallTo(() => fakeScope.OuterScope).Returns(globalScope);

            var boundScope = new BoundScope<SymbolNode>(fakeScope, fakeNode);
            boundScope.Name.ShouldBe("global/FakeScope");
        }

        [TestMethod]
        public void Should_resolve_defined_symbol()
        {
            var symbol = new SymbolNode("foo");
            var scope = new AnonymousScope<SymbolNode>(null);

            scope.Define(symbol.Symbol, symbol);

            scope.Resolve("foo").ShouldBe(symbol);
        }

        [TestMethod]
        public void Should_resolve_defined_symbol_in_parent_scope()
        {
            var symbol = new SymbolNode("foo");
            var parentScope = new AnonymousScope<SymbolNode>(null);
            var scope = new AnonymousScope<SymbolNode>(parentScope);

            parentScope.Define(symbol.Symbol, symbol);

            scope.Resolve("foo").ShouldBe(symbol);
        }

        [TestMethod]
        public void Should_return_globalscope_for_root_node()
        {
            var rootNode = new RootNode();

            var builder = CreateScopeBuilder(new GlobalScope<SymbolNode>());
            var scope = builder.Visit(rootNode) as BoundScope<SymbolNode>;
            scope.ShouldBeOfType<BoundScope<SymbolNode>>();
            scope.ShouldNotBe(null);

            scope.Node.ShouldBe(rootNode);
            scope.TargetScope.ShouldBeOfType<GlobalScope<SymbolNode>>();
        }

        [TestMethod]
        public void Should_scope_nested_operator_symbols_but_not_define_them()
        {
            var greatGrandChild = new OperatorNode("+");
            var grandChildren = new INode<AstNode>[] { new ListNode(greatGrandChild) };

            var children = new INode<AstNode>[] { new ListNode(grandChildren) };
            var fakeScope = A.Fake<IScope<SymbolNode>>();

            var rootNode = new RootNode(children);

            var builder = CreateScopeBuilder(fakeScope);
            var rootScope = builder.Visit(rootNode);
            rootScope.ShouldNotBe(null);
            rootScope.Descendants().OfType<BoundScope<SymbolNode>>().Any().ShouldBe(true);

            A.CallTo(() => fakeScope.Define("+", greatGrandChild)).MustNotHaveHappened();
        }

        [TestMethod]
        public void Should_scope_nested_symbols_but_not_define_them()
        {
            var greatGrandChild = new SymbolNode("ghi");
            var grandChildren = new AstNode[] { new SymbolNode("abc"), new SymbolNode("def"), new ListNode(greatGrandChild) };

            var children = new AstNode[] { new ListNode(grandChildren), };
            var fakeScope = A.Fake<IScope<SymbolNode>>();

            var rootNode = new RootNode(children);

            var builder = CreateScopeBuilder(fakeScope);
            var rootScope = builder.Visit(rootNode);
            rootScope.ShouldNotBe(null);
            rootScope.Descendants().OfType<BoundScope<SymbolNode>>().Any().ShouldBe(true);

            A.CallTo(() => fakeScope.Define("abc", (SymbolNode)grandChildren[0])).MustNotHaveHappened();
            A.CallTo(() => fakeScope.Define("def", (SymbolNode)grandChildren[1])).MustNotHaveHappened();
            A.CallTo(() => fakeScope.Define("ghi", (SymbolNode)greatGrandChild)).MustNotHaveHappened();
        }

        [TestMethod]
        public void Should_scope_symbols_but_not_define_them()
        {
            var children = new AstNode[] { new SymbolNode("abc"), new SymbolNode("def") };
            var fakeScope = A.Fake<IScope<SymbolNode>>();

            var rootNode = new RootNode(children);

            var builder = CreateScopeBuilder(fakeScope);
            var scope = builder.Visit(rootNode);
            scope.ShouldNotBe(null);

            scope.Descendants().OfType<BoundScope<SymbolNode>>().Any().ShouldBe(true);
            A.CallTo(() => fakeScope.Define("abc", (SymbolNode)children[0])).MustNotHaveHappened();
            A.CallTo(() => fakeScope.Define("def", (SymbolNode)children[1])).MustNotHaveHappened();
        }

        [TestMethod]
        public void Should_scope_parameters()
        {
            var body = new ListNode();
            var methodDefinitionNode = new MethodDefinitionNode("sayMessage", new ParameterDefinitionNode[] { new ParameterDefinitionNode("message"), }, body);

            var globalScope = new GlobalScope<SymbolNode>();

            var builder = CreateScopeBuilder(globalScope);
            var resultScope = builder.GetScope(methodDefinitionNode, globalScope);
            resultScope.Resolve("message").ShouldNotBe(null);
            resultScope.Resolve("message").ShouldBeOfType<ParameterDefinitionNode>();
        }

        [TestMethod]
        public void Should_scope_method_definition()
        {
            var body = new ListNode();
            var methodDefinitionNode = new MethodDefinitionNode("sayMessage", new ParameterDefinitionNode[] { new ParameterDefinitionNode("message"), }, body);

            var globalScope = new GlobalScope<SymbolNode>();

            var builder = CreateScopeBuilder(globalScope);
            var resultScope = builder.GetScope(methodDefinitionNode, globalScope);
            resultScope.Resolve("sayMessage").ShouldNotBe(null);
            resultScope.Resolve("sayMessage").ShouldBeOfType<MethodDefinitionNode>();
            resultScope.Name.ShouldBe("global/sayMessage");
        }

        [TestMethod]
        public void Should_scope_method_body()
        {
            var fooNode = new SymbolNode("foo");
            var body = new ListNode(fooNode);
            var methodDefinitionNode = new MethodDefinitionNode("sayMessage", new ParameterDefinitionNode[] { new ParameterDefinitionNode("message"), }, body);

            var globalScope = new GlobalScope<SymbolNode>();

            var builder = CreateScopeBuilder(globalScope);
            var resultScope = builder.GetScope(methodDefinitionNode, globalScope);
            resultScope.Descendants().OfType<BoundScope<SymbolNode>>().Any(scope => scope.Node == fooNode).ShouldBe(true);
        }

        [TestMethod]
        public void Should_not_scope_atom_nodes()
        {
            var fooNode = new NumberNode("123");
            var body = new ListNode(fooNode);
            var methodDefinitionNode = new MethodDefinitionNode("sayMessage", new ParameterDefinitionNode[] { new ParameterDefinitionNode("message"), }, body);

            var globalScope = new GlobalScope<SymbolNode>();

            var builder = CreateScopeBuilder(globalScope);
            var resultScope = builder.GetScope(methodDefinitionNode, globalScope);
            resultScope.Descendants().OfType<BoundScope<SymbolNode>>().Any(scope => scope.Node == fooNode).ShouldBe(false);
        }

        [TestMethod]
        public void Should_scope_variable_definitions()
        {
            var body = new ListNode(new VariableDefinitionNode("someBooleanValue", new FalseNode()));
            var methodDefinitionNode = new MethodDefinitionNode("sayMessage", new ParameterDefinitionNode[] { new ParameterDefinitionNode("message"), }, body);

            var globalScope = new GlobalScope<SymbolNode>();

            var builder = CreateScopeBuilder(globalScope);
            var resultScope = builder.GetScope(methodDefinitionNode, globalScope);
            resultScope.Resolve("message").ShouldNotBe(null);
            resultScope.Resolve("message").ShouldBeOfType<ParameterDefinitionNode>();
            resultScope.Resolve("someBooleanValue").ShouldNotBe(null);

            var symbol = resultScope.Resolve("someBooleanValue") as VariableDefinitionNode;
            symbol.ShouldNotBe(null);
            symbol.Value.ShouldBeOfType<FalseNode>();
        }

        [TestMethod]
        public void Should_return_root_scope_by_default()
        {
            var globalScope = new GlobalScope<SymbolNode>();
            var node = new SymbolNode("abc");

            var builder = CreateScopeBuilder(globalScope);
            var scope = builder.Visit(node);
            scope.ShouldBeOfType<BoundScope<SymbolNode>>();
            scope.TargetScope.ShouldBe(globalScope);
            scope.Node.ShouldBe(node);
        }

        [TestMethod]
        public void Should_be_able_to_determine_unresolved_symbols()
        {
            var globalScope = new GlobalScope<SymbolNode>();
            var builder = CreateScopeBuilder(globalScope);

            var undefinedSymbol = new SymbolNode("unresolvedSymbol123");
            var children = new List<INode<AstNode>>() { new MethodDefinitionNode("foo", new List<ParameterDefinitionNode>(), new TrueNode()), new ListNode(undefinedSymbol), new ListNode(new SymbolNode("foo")) };

            var rootNode = new RootNode(children);
            var rootScope = builder.GetScope(rootNode);
            var boundScopes = rootScope.UnresolvedSymbols().OfType<IBoundScope<SymbolNode>>();
            boundScopes.ShouldNotBe(null);

            boundScopes.Select(b => b.Node).OfType<SymbolNode>().Any(s => s.Symbol == undefinedSymbol.Symbol).ShouldBe(true);
            boundScopes.Select(b => b.Node).OfType<SymbolNode>().Any(s => s.Symbol == "foo").ShouldBe(false);

            globalScope.Resolve("foo").ShouldNotBe(null);
        }

        private ScopeBuilder<SymbolNode> CreateScopeBuilder(IScope<SymbolNode> scope)
        {
            return new ScopeBuilder<SymbolNode>(scope, new SymbolNodeScopingStrategy());
        }
    }
}
