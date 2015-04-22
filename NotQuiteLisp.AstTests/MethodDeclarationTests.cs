using System.Linq;
using NotQuiteLisp.AST;
using NotQuiteLisp.Core;
using Shouldly;

namespace NotQuiteLisp.AstTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Visitors;

    [TestClass]
    public class MethodDeclarationTests
    {
        [TestMethod]
        public void Should_transform_defn_list_into_method_declaration()
        {
            var inputText = "(defn sayName [name] (WriteLine name))";
            var originalRootNode = inputText.CreateAstNodes();

            var converter = new MethodDeclarationConverter();
            var outputNode = converter.Visit(originalRootNode);

            var descendants = outputNode.Descendants().ToArray();
            descendants.Any(d => d is MethodDefinitionNode).ShouldBe(true);

            var methodDef = descendants.First(d => d is MethodDefinitionNode) as MethodDefinitionNode;
            methodDef.ShouldNotBe(null);

            // Match the parameters
            methodDef.Parameters.First().Symbol.ShouldBe("name");

            // Match the method body
            var methodBody = methodDef.MethodBody as ListNode;
            methodBody.ShouldNotBe(null);

            methodBody.Children.First().ShouldBeOfType<SymbolNode>();

            var firstChild = (SymbolNode)methodBody.Children.First();
            firstChild.Symbol.ShouldBe("WriteLine");
        }
    }
}