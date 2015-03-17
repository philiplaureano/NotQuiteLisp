using System;
using System.Linq;
using ANTLR4.ParserHelpers;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotQuiteLisp.Parser;
using Shouldly;

namespace NotQuiteLisp.ParserTests
{
    [TestClass]
    public class SyntaxTests
    {
        [TestMethod]
        public void Should_parse_compile_unit()
        {
            var inputText = "(foo)";
            var tree = inputText.ParseWith<NqlLanguage>();

            Assert.IsInstanceOfType(tree, typeof(NQLParser.CompileUnitContext));
        }

        [TestMethod]
        public void Should_parse_number()
        {
            var inputText = "1";
            var expectedText = "1";
            var expectedRule = NQLParser.NUMBER;

            TestTerminalParse(inputText, expectedRule, expectedText);
        }

        [TestMethod]
        public void Should_parse_symbol()
        {
            var inputText = "abcd";
            var expectedText = "abcd";
            var expectedRule = NQLParser.SYMBOL;

            TestTerminalParse(inputText, expectedRule, expectedText);
        }

        [TestMethod]
        public void Should_parse_string()
        {
            var inputText = "\"this is a string\"";
            var expectedText = "\"this is a string\"";
            var expectedRule = NQLParser.STRING;

            TestTerminalParse(inputText, expectedRule, expectedText);
        }

        [TestMethod]
        public void Should_parse_child_list()
        {
            var inputText = "(foo)";
            var treeRoot = inputText.ParseWith<NqlLanguage>();

            var rootElement = treeRoot.GetChild(0);
            rootElement.ShouldBeOfType<NQLParser.ElementContext>();

            // There should be at least one child list
            rootElement.ChildCount.ShouldBeGreaterThan(0);
            rootElement.GetChild(0).ShouldBeOfType<NQLParser.ListContext>();
        }

        [TestMethod]
        public void Should_parse_symbol_from_list()
        {
            var inputText = "(foo)";
            var expectedText = "foo";
            var expectedRule = NQLParser.SYMBOL;

            TestListEmbeddedTerminalRule(inputText, expectedRule, expectedText);
        }

        [TestMethod]
        public void Should_parse_number_from_list()
        {
            var inputText = "(1)";
            var expectedText = "1";
            var expectedRule = NQLParser.NUMBER;

            TestListEmbeddedTerminalRule(inputText, expectedRule, expectedText);
        }

        private static void TestListEmbeddedTerminalRule(string inputText, int expectedRule, string expectedText)
        {
            var treeRoot = inputText.ParseWith<NqlLanguage>();
            var rootElement = treeRoot.GetChild(0);

            rootElement.ChildCount.ShouldBeGreaterThan(0);

            var listElement = rootElement.GetChild(0);
            listElement.ShouldBeOfType<NQLParser.ListContext>();

            // There should be the LPAREN, element, and RPAREN tokens parsed
            var listChildren = listElement.Children().ToArray();
            listChildren.Length.ShouldBe(3);
            listChildren[0].ShouldBeOfType<TerminalNodeImpl>();
            listChildren[1].ShouldBeOfType<NQLParser.ElementContext>();
            listChildren[2].ShouldBeOfType<TerminalNodeImpl>();

            // Drill into the Atom node
            var atomNode = listChildren[1].Children().First();
            atomNode.ShouldBeOfType<NQLParser.AtomContext>();

            atomNode.ChildCount.ShouldBeGreaterThan(0);

            // The target node is the first terminal node
            var targetNode = atomNode.Children().First();
            var payload = (CommonToken)targetNode.Payload;

            payload.Type.ShouldBe(expectedRule);
            payload.Text.ShouldBe(expectedText);
        }

        private static void TestTerminalParse(string inputText, object expectedRule, string expectedText)
        {
            var tree = inputText.ParseWith<NqlLanguage>();
            tree.ChildCount.ShouldBe(2);

            // Check for a parse error
            var rootElementNode = tree.Children().First();
            rootElementNode.GetType().ShouldNotBe(typeof(ErrorNodeImpl));

            var atomNode = rootElementNode.Children().First();

            var terminalNode = atomNode.Children().First();

            var token = (CommonToken) terminalNode.Payload;

            token.Type.ShouldBe(expectedRule);
            token.Text.ShouldBe(expectedText);
        }
    }
}
