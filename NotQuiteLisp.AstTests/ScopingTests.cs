﻿using System;
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
        public void Should_scope_nested_operator_symbols_but_not_define_them()
        {
            var greatGrandChild = new OperatorNode("+");
            var grandChildren = new INode<AstNode>[] { new ListNode(greatGrandChild) };

            var children = new INode<AstNode>[] { new ListNode(grandChildren) };
            var fakeScope = A.Fake<IScope>();

            var rootNode = new RootNode(children);

            var builder = new ScopeBuilder(fakeScope);
            var rootScope = builder.Visit(rootNode);
            rootScope.ShouldNotBe(null);
            rootScope.Descendants().OfType<BoundScope>().Any().ShouldBe(true);

            A.CallTo(() => fakeScope.Define((SymbolNode)greatGrandChild)).MustNotHaveHappened();
        }
        
        [TestMethod]
        public void Should_scope_nested_symbols_but_not_define_them()
        {
            var greatGrandChild = new SymbolNode("ghi");
            var grandChildren = new AstNode[] { new SymbolNode("abc"), new SymbolNode("def"), new ListNode(greatGrandChild) };

            var children = new AstNode[] { new ListNode(grandChildren), };
            var fakeScope = A.Fake<IScope>();

            var rootNode = new RootNode(children);

            var builder = new ScopeBuilder(fakeScope);
            var rootScope = builder.Visit(rootNode);
            rootScope.ShouldNotBe(null);
            rootScope.Descendants().OfType<BoundScope>().Any().ShouldBe(true);

            A.CallTo(() => fakeScope.Define((SymbolNode)grandChildren[0])).MustNotHaveHappened();
            A.CallTo(() => fakeScope.Define((SymbolNode)grandChildren[1])).MustNotHaveHappened();
            A.CallTo(() => fakeScope.Define((SymbolNode)greatGrandChild)).MustNotHaveHappened();
        }

        [TestMethod]
        public void Should_scope_symbols_but_not_define_them()
        {
            var children = new AstNode[] { new SymbolNode("abc"), new SymbolNode("def") };
            var fakeScope = A.Fake<IScope>();

            var rootNode = new RootNode(children);

            var builder = new ScopeBuilder(fakeScope);
            var scope = builder.Visit(rootNode);
            scope.ShouldNotBe(null);

            scope.Descendants().OfType<BoundScope>().Any().ShouldBe(true);
            A.CallTo(() => fakeScope.Define((SymbolNode)children[0])).MustNotHaveHappened();
            A.CallTo(() => fakeScope.Define((SymbolNode)children[1])).MustNotHaveHappened();
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

        [TestMethod]
        public void Should_scope_method_definition()
        {
            var body = new ListNode();
            var methodDefinitionNode = new MethodDefinitionNode("sayMessage", new ParameterDefinitionNode[] { new ParameterDefinitionNode("message"), }, body);

            var globalScope = new GlobalScope();

            var builder = new ScopeBuilder(globalScope);
            var resultScope = builder.GetScope(methodDefinitionNode, globalScope);
            resultScope.Resolve("sayMessage").ShouldNotBe(null);
            resultScope.Resolve("sayMessage").ShouldBeOfType<MethodDefinitionNode>();
        }

        [TestMethod]
        public void Should_scope_variable_definitions()
        {
            var body = new ListNode(new VariableDefinitionNode("someBooleanValue", new FalseNode()));
            var methodDefinitionNode = new MethodDefinitionNode("sayMessage", new ParameterDefinitionNode[] { new ParameterDefinitionNode("message"), }, body);

            var globalScope = new GlobalScope();

            var builder = new ScopeBuilder(globalScope);
            var resultScope = builder.GetScope(methodDefinitionNode, globalScope);
            resultScope.Resolve("message").ShouldNotBe(null);
            resultScope.Resolve("message").ShouldBeOfType<ParameterDefinitionNode>();
            resultScope.Resolve("someBooleanValue").ShouldNotBe(null);

            var symbol = resultScope.Resolve("someBooleanValue") as VariableDefinitionNode;
            symbol.ShouldNotBe(null);
            symbol.Value.ShouldBeOfType<FalseNode>();
        }
    }
}
