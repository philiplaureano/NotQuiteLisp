namespace NotQuiteLisp.AstTests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NotQuiteLisp.AST;
    using NotQuiteLisp.Core;

    using Shouldly;

    [TestClass]
    public class LateBindingTests
    {
        [TestMethod]
        public void Should_call_compatible_method()
        {
            var sampleVisitor = new SampleVisitor();
            var symbol = new SymbolNode("foo");
            sampleVisitor.Invoke("Visit", symbol);

            sampleVisitor.NumberOfTimesCalled.ShouldBe(1);
        }        
    }
}