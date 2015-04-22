using System.Collections.Generic;
using System.Linq;
using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    public class VariableDeclarationConverter : ListConverter
    {
        public VariableDeclarationConverter()
            : base(2, "def")
        {
        }

        protected override AstNode CreateConvertedNode(AstNode originalNode, IEnumerable<AstNode> children)
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