namespace NotQuiteLisp.AstTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NotQuiteLisp.AST;
    using NotQuiteLisp.Core;

    using Shouldly;

    [TestClass]
    public class NodeExtensionTests
    {
        [TestMethod]
        public void Should_be_able_to_determine_tree_height()
        {
            var children = new AstNode[]
                               {
                                   new SymbolNode("foo"), 
                                   new ListNode(
                                       new SymbolNode("abc"), 
                                       new SymbolNode("def")
                                       )
                               };
            var root = new RootNode(children);
            root.Height().ShouldBe(2);
        }
    }
}