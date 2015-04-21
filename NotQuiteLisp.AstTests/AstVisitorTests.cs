using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotQuiteLisp.AST;
using NotQuiteLisp.Visitors;
using Shouldly;

namespace NotQuiteLisp.AstTests
{
    [TestClass]
    public class AstVisitorTests
    {
        [TestMethod]
        public void Should_call_most_derived_visit_method()
        {
            var sampleVisitor = new SampleVisitor();
            IVisitor<AstNode, int> visitor = sampleVisitor;
            visitor.Visit(new SymbolNode("foo"));

            sampleVisitor.NumberOfTimesCalled.ShouldBe(1);
        }

        [TestMethod]
        public void Should_not_throw_an_exception_when_error_suppression_is_enabled()
        {
            var sampleVisitor = new SampleSilentVisitor();
            IVisitor<AstNode, int> visitor = sampleVisitor;
            visitor.Visit(new SymbolNode("bar"));

            sampleVisitor.NumberOfTimesCalled.ShouldBe(1);
        }
    }
}