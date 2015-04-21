using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotQuiteLisp.AST;
using NotQuiteLisp.Core;
using NotQuiteLisp.Visitors;
using Shouldly;

namespace NotQuiteLisp.AstTests
{
    [TestClass]
    public class VariableDeclarationTests
    {
        [TestMethod]
        public void Should_transform_def_symbol_node_into_variable_declaration_node()
        {
            var inputText = "(def text \"Hello, World!\")";
            var originalRootNode = inputText.CreateAstNodes();

            IVisitor<AstNode, AstNode> converter = new VariableDeclarationConverter();
            var outputNode = converter.Visit(originalRootNode);

            outputNode.Descendants().Any(d => d is VariableDeclarationNode).ShouldBe(true);
            var declarationNode =
                outputNode.Descendants()
                    .Where(d => d is VariableDeclarationNode)
                    .Cast<VariableDeclarationNode>()
                    .First();

            declarationNode.Symbol.ShouldBe("text");
            declarationNode.Value.ShouldBeOfType<StringNode>();

            var stringNode = (StringNode)declarationNode.Value;
            stringNode.Text.ShouldBe("Hello, World!");
        }
    }
}