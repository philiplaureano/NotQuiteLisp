namespace NotQuiteLisp.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AST;
    using Visitors;

    public class MethodDeclarationConverter : ListConverter
    {
        public MethodDeclarationConverter()
            : base(3, "defn")
        {
        }

        protected override AstNode CreateConvertedNode(AstNode originalNode, IEnumerable<AstNode> children)
        {
            var childNodes = children.ToArray();

            // The first child should be the symbol for the method name
            var nameNode = childNodes[1] as SymbolNode;
            if (nameNode == null)
                return originalNode.Clone();

            // The second child needs to be a vector node 
            // with the list of symbols for params
            var vectorNode = childNodes[2] as VectorNode;
            if (vectorNode == null)
                return originalNode.Clone();

            // All vector node children must be symbols
            if (vectorNode.Children.Any(child => child.GetType() != typeof(SymbolNode)))
                return originalNode.Clone();

            // Determine the parameters
            var parameters = vectorNode.Children.Cast<SymbolNode>().Select(child => new ParameterDefinitionNode(child.Symbol));

            // The third child is the body of the method declaration itself

            return new MethodDefinitionNode(nameNode.Symbol, parameters, childNodes[3]);
        }
    }
}