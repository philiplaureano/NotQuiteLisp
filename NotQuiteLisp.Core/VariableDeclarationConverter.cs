using System.Collections.Generic;
using System.Linq;
using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    using NotQuiteLisp.AST.Interfaces;

    public class VariableDeclarationConverter : SpecificListConverter
    {
        public VariableDeclarationConverter()
            : base(2, "def")
        {
        }

        protected override INode<AstNode> CreateConvertedNode(INode<AstNode> originalNode, IEnumerable<INode<AstNode>> children)
        {
            var childNodes = children.ToArray();
            var variableNameNode = childNodes[1] as SymbolNode;
            var valueNode = childNodes[2];
            if (variableNameNode == null || valueNode == null)
                return new ListNode(childNodes);

            var declaration = new VariableDefinitionNode(variableNameNode.Symbol, valueNode.Clone());
            return declaration;
        }
    }
}