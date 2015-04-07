using System.Linq;
using ANTLR4.ParserHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotQuiteLisp.AST;
using NotQuiteLisp.Core;
using NotQuiteLisp.Parser;
using NotQuiteLisp.Visitors;
using Shouldly;

namespace NotQuiteLisp.AstTests
{
    [TestClass]
    public class ParentNodeMappingTests
    {
        [TestMethod]
        public void Should_be_able_to_determine_parent_from_given_node()
        {
            var inputText = "(def my-name \"Me\")";
            
            var rootNode = inputText.CreateAstNodes();
            var nodeMapper = new TreeMapper();
            var map = nodeMapper.CreateMap(rootNode);

            var children = rootNode.Children;
            foreach (var child in children)
            {
                map.GetParentFor(child).ShouldBe(rootNode);
            }
        }

        [TestMethod]
        public void Should_be_able_to_determine_parent_from_nested_node()
        {
            var inputText = "(keyword1 keyword2 (keyword3 keyword4 (keyword5 keyword6)))";

            var rootNode = inputText.CreateAstNodes();
            var nodeMapper = new TreeMapper();
            var map = nodeMapper.CreateMap(rootNode);

            var topNode = (ListNode)rootNode.Children.First();

            var children = topNode.Children;
            foreach (var child in children)
            {
                map.GetParentFor(child).ShouldBe(topNode);
            }
        }
    }
}