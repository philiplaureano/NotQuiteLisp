using System;
using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    public class BooleanEvaluator : IFunction
    {
        public AstNode Eval(AstNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            if (node is NumberNode)
                return Eval(node as NumberNode);

            if (node is NilNode)
                return BooleanNode.False;

            if (node is BooleanNode)
                return node;

            if (node is StringNode)
                return Eval(node as StringNode);

            // Unhandled node types will cause an exception by default
            throw new NotSupportedException(string.Format("Node type {0} not supported", node.GetType().Name));
        }

        private AstNode Eval(NumberNode node)
        {
            int value;
            var parsed = int.TryParse(node.Number, out value);

            return parsed && value > 0 ? BooleanNode.True : BooleanNode.False;
        }

        private AstNode Eval(StringNode node)
        {
            var text = node.Text;
            return string.IsNullOrEmpty(text) ? BooleanNode.False : BooleanNode.True;
        }
    }
}