using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotQuiteLisp.AST;
using NotQuiteLisp.Core;
using Shouldly;

namespace NotQuiteLisp.AstTests
{
    [TestClass]
    public class BooleanEvaluatorTests
    {
        [TestMethod]
        public void Should_evaluate_true_for_a_non_zero_number_node()
        {
            var inputNode = new NumberNode("1");
            var evaluator = new BooleanEvaluator();
            var result = evaluator.Eval(inputNode);
            result.ShouldBeOfType<TrueNode>();
        }

        [TestMethod]
        public void Should_evaluate_true_for_non_empty_string_node()
        {
            var inputNode = new StringNode("abc");
            var evaluator = new BooleanEvaluator();
            var result = evaluator.Eval(inputNode);
            result.ShouldBeOfType<TrueNode>();
        }

        [TestMethod]
        public void Should_evaluate_false_for_empty_string_node()
        {
            var inputNode = new StringNode(string.Empty);
            var evaluator = new BooleanEvaluator();
            var result = evaluator.Eval(inputNode);
            result.ShouldBeOfType<FalseNode>();
        }

        [TestMethod]
        public void Should_evaluate_false_for_null_string_node()
        {
            var inputNode = new StringNode(null);
            var evaluator = new BooleanEvaluator();
            var result = evaluator.Eval(inputNode);
            result.ShouldBeOfType<FalseNode>();
        }

        [TestMethod]
        public void Should_evaluate_false_for_a_zero_number_node()
        {
            var inputNode = new NumberNode("0");
            var evaluator = new BooleanEvaluator();
            var result = evaluator.Eval(inputNode);
            result.ShouldBeOfType<FalseNode>();
        }

        [TestMethod]
        public void Should_evaluate_false_for_invalid_number_node()
        {
            var inputNode = new NumberNode("abc");
            var evaluator = new BooleanEvaluator();
            var result = evaluator.Eval(inputNode);
            result.ShouldBeOfType<FalseNode>();
        }

        [TestMethod]
        public void Should_evaluate_false_for_nil()
        {
            var inputNode = new NilNode();
            var evaluator = new BooleanEvaluator();
            var result = evaluator.Eval(inputNode);
            result.ShouldBeOfType<FalseNode>();
        }

        [TestMethod]
        public void Should_evaluate_false_for_false()
        {
            var inputNode = new FalseNode();
            var evaluator = new BooleanEvaluator();
            var result = evaluator.Eval(inputNode);
            result.ShouldBeOfType<FalseNode>();
        }

        [TestMethod]
        public void Should_evaluate_true_for_true()
        {
            var inputNode = new TrueNode();
            var evaluator = new BooleanEvaluator();
            var result = evaluator.Eval(inputNode);
            result.ShouldBeOfType<TrueNode>();
        }
    }
}