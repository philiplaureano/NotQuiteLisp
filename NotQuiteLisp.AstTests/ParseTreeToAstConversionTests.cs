using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotQuiteLisp.Core;
using NotQuiteLisp.Parser;
namespace NotQuiteLisp.AstTests
{
    using ANTLR4.ParserHelpers;

    using NotQuiteLisp.Parser;
    using NotQuiteLisp.Visitors;

    using NotQuiteLisp.AST;

    using Shouldly;

    [TestClass]
    public class ParseTreeToAstConversionTests
    {
        [TestMethod]
        public void Should_generate_root_node_from_compile_unit()
        {
            var inputText = "(+ 1 2)";
            var tree = (NQLParser.CompileUnitContext)inputText.ParseWith<NqlLanguage>();

            var converter = new ParseTreeConverter();
            var outputNode = converter.VisitCompileUnit(tree);
            (outputNode != null).ShouldBe(true);

            outputNode.ShouldBeOfType<RootNode>();
        }

        [TestMethod]
        public void Should_generate_true_boolean_node()
        {
            var inputText = "(true)";
            var converter = new ParseTreeConverter();
            var tree = inputText.ParseWith<NqlLanguage>();
            
            var rootNode = (RootNode)converter.Visit(tree);
            var listNode = rootNode.Children.Cast<ListNode>().First();

            listNode.ShouldNotBe(null);
            
            var childNodes = listNode.Children.ToArray();
            childNodes.Count().ShouldBe(1);

            childNodes[0].ShouldBeOfType<TrueNode>();
        }

        [TestMethod]
        public void Should_generate_false_boolean_node()
        {
            var inputText = "(false)";
            var converter = new ParseTreeConverter();
            var tree = inputText.ParseWith<NqlLanguage>();

            var rootNode = (RootNode)converter.Visit(tree);
            var listNode = rootNode.Children.Cast<ListNode>().First();

            listNode.ShouldNotBe(null);

            var childNodes = listNode.Children.ToArray();
            childNodes.Count().ShouldBe(1);

            childNodes[0].ShouldBeOfType<FalseNode>();            
        }

        [TestMethod]
        public void Should_generate_set_node()
        {
            var inputText = "#{1 2 3}";
            var converter = new ParseTreeConverter();
            var tree = inputText.ParseWith<NqlLanguage>();
            AstNode root = converter.Visit(tree);

            root.ShouldNotBe(null);
            root.Children.Count().ShouldBe(1);

            var setNode = root.Children.Cast<SetNode>().First();
            setNode.Children.Count().ShouldBe(3);
            setNode.Children.All(child => child.GetType() == typeof(NumberNode)).ShouldBe(true);
        }

        [TestMethod]
        public void Should_generate_keyword_node()
        {
            var inputText = ":keyword123";
            var converter = new ParseTreeConverter();
            var tree = inputText.ParseWith<NqlLanguage>();
            AstNode root = converter.Visit(tree);

            root.ShouldNotBe(null);
            root.Children.Count().ShouldBe(1);

            var keywordNode = (KeywordNode)root.Children.First();
            keywordNode.Keyword.ShouldBe(":keyword123");
        }

        [TestMethod]
        public void Should_generate_vector_node()
        {
            var inputText = "[1 2 3]";

            var tree = inputText.ParseWith<NqlLanguage>();
            var converter = new ParseTreeConverter();
            AstNode root = converter.Visit(tree);

            root.ShouldNotBe(null);
            root.Children.Count().ShouldBe(1);

            var vectorNode = root.Children.First() as VectorNode;
            vectorNode.ShouldNotBe(null);
            vectorNode.Children.Count().ShouldBe(3);

            var numberNodes = vectorNode.Children.Cast<NumberNode>().ToArray();
            numberNodes[0].Number.ShouldBe("1");
            numberNodes[1].Number.ShouldBe("2");
            numberNodes[2].Number.ShouldBe("3");
        }

        [TestMethod]
        public void Should_generate_vector_node_from_list()
        {
            var inputText = "(defn sayName [name] (WriteLine name))";
            var rootNode = inputText.CreateAstNodes();
            rootNode.ShouldNotBe(null);

            rootNode.Descendants().Any(d => d is VectorNode).ShouldBe(true);
        }

        [TestMethod]
        public void Should_generate_operator_node_from_compile_unit()
        {
            var inputText = "(+)";

            var converter = new ParseTreeConverter();

            var tree = inputText.ParseWith<NqlLanguage>();
            AstNode root = converter.Visit(tree);

            root.ShouldNotBe(null);
            root.Children.Count().ShouldBe(1);

            var listNode = root.Children.First();
            listNode.ShouldBeOfType<ListNode>();

            var targetNode = (OperatorNode)listNode.Children.First();
            targetNode.Operator.ShouldBe("+");
        }

        [TestMethod]
        public void Should_unwrap_atom_from_element()
        {
            var inputText = "(123)";
            var tree = inputText.ParseWith<NqlLanguage>();

            var elementDescendant = tree.Descendants()
                .First(d => d.GetType().Name.StartsWith("Element"));

            var converter = new ParseTreeConverter();
            var outputNode = converter.Visit(elementDescendant) as NumberNode;
            Assert.IsNotNull(outputNode);
            outputNode.Number.ShouldBe("123");
        }

        [TestMethod]
        public void Should_unwrap_string_from_atom()
        {
            var inputText = "(\"text1\")";
            var tree = inputText.ParseWith<NqlLanguage>();

            var atomDescendant = tree.Descendants()
                .First(d => d.GetType().Name.StartsWith("Atom"));

            var converter = new ParseTreeConverter();
            var outputNode = converter.Visit(atomDescendant) as StringNode;
            Assert.IsNotNull(outputNode);
            outputNode.Text.ShouldBe("text1");
        }

        [TestMethod]
        public void Should_unwrap_number_from_atom()
        {
            var inputText = "(123)";
            var tree = inputText.ParseWith<NqlLanguage>();

            var atomDescendant = tree.Descendants()
                .First(d => d.GetType().Name.StartsWith("Atom"));

            var converter = new ParseTreeConverter();
            var outputNode = converter.Visit(atomDescendant) as NumberNode;
            Assert.IsNotNull(outputNode);
            outputNode.Number.ShouldBe("123");
        }

        [TestMethod]
        public void Should_unwrap_symbol_from_atom()
        {
            var inputText = "(abcd)";
            var tree = inputText.ParseWith<NqlLanguage>();

            var atomDescendant = tree.Descendants()
                .First(d => d.GetType().Name.StartsWith("Atom"));

            var converter = new ParseTreeConverter();
            var outputNode = converter.Visit(atomDescendant) as SymbolNode;
            Assert.IsNotNull(outputNode);
            outputNode.Symbol.ShouldBe("abcd");
        }

        [TestMethod]
        public void Should_unwrap_operator_from_atom()
        {
            var inputText = "(+)";
            var tree = inputText.ParseWith<NqlLanguage>();

            var atomDescendant = tree.Descendants()
                .First(d => d.GetType().Name.StartsWith("Atom"));

            var converter = new ParseTreeConverter();
            var outputNode = converter.Visit(atomDescendant) as OperatorNode;
            Assert.IsNotNull(outputNode);
            outputNode.Operator.ShouldBe("+");
        }

        [TestMethod]
        public void Should_create_list_from_element()
        {
            var inputText = "(+ 1 2)";
            var tree = inputText.ParseWith<NqlLanguage>();
            var elementDescendant = tree.Descendants()
                .First(d => d.GetType().Name.StartsWith("Element") && d.Children().Any(c => c.GetType().Name.StartsWith("List")));

            var converter = new ParseTreeConverter();
            var outputNode = converter.Visit(elementDescendant) as ListNode;
            Assert.IsNotNull(outputNode);

            outputNode.Children.Count().ShouldBe(3);
        }

        [TestMethod]
        public void Should_create_map()
        {
            var inputText = "{:language \"Clojure\", :creator \"Rich Hickey\"}";
            var tree = inputText.ParseWith<NqlLanguage>();
            var converter = new ParseTreeConverter();
            var rootNode = (RootNode)converter.Visit(tree);
            rootNode.Children.Count().ShouldBe(1);

            var mapNode = rootNode.Children.Cast<MapNode>().First();
            mapNode.ShouldNotBe(null);

            var entries = mapNode.Entries.ToArray();
            entries.Count().ShouldBe(2);
        }

        [TestMethod]
        public void Should_create_quoted_list()
        {
            var inputText = "'(+ 1 2)";
            var tree = inputText.ParseWith<NqlLanguage>();
            var elementDescendant = tree.Descendants()
                .First(d => d.GetType().Name.StartsWith("Element") && d.Children().Any(c => c.GetType().Name.StartsWith("Quoted")));

            var converter = new ParseTreeConverter();
            var outputNode = converter.Visit(elementDescendant) as QuotedListNode;
            Assert.IsNotNull(outputNode);

            outputNode.Children.Count().ShouldBe(1);
        }

        [TestMethod]
        public void Should_create_nil_nodes()
        {
            var inputText = "(nil 1)";
            var tree = inputText.ParseWith<NqlLanguage>();
            var converter = new ParseTreeConverter();

            var root = (RootNode)converter.Visit(tree);
            var listNode = root.Children.Cast<ListNode>().First(c => c != null);
            var listElements = listNode.Children.ToArray();

            listElements[0].GetType().ShouldBe(typeof(NilNode));
            listElements[1].GetType().ShouldBe(typeof(NumberNode));
        }
    }
}
