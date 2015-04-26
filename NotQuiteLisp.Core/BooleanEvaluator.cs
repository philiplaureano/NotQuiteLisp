using System;
using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    using NotQuiteLisp.AST.Interfaces;

    public class BooleanEvaluator : IFunction
    {
        public INode<AstNode> Eval(INode<AstNode> node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            if (node is NumberNode)
                return Eval(node as NumberNode);

            if (node is NilNode)
                return new FalseNode();

            if (node is BooleanNode)
                return node;

            if (node is StringNode)
                return Eval(node as StringNode);

            // Unhandled node types will cause an exception by default
            throw new NotSupportedException(string.Format("Node type {0} not supported", node.GetType().Name));
        }

        private INode<AstNode> Eval(NumberNode node)
        {
            int value;
            var parsed = int.TryParse(node.Number, out value);

            return parsed && value > 0 ? (INode<AstNode>)new TrueNode() : new FalseNode();
        }

        private INode<AstNode> Eval(StringNode node)
        {
            var text = node.Text;
            return string.IsNullOrEmpty(text) ? (INode<AstNode>)new FalseNode() : new TrueNode();
        }
    }
}