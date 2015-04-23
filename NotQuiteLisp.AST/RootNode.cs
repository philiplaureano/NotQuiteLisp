using System.Linq;
using NotQuiteLisp.AST.Interfaces;

namespace NotQuiteLisp.AST
{
    using System.Collections.Generic;

    public sealed class RootNode : AstNode, IScopeContainer
    {
        public RootNode()
        {
        }

        public RootNode(IEnumerable<AstNode> childNodes)
            : base(childNodes)
        {
        }

        public override AstNode Clone()
        {
            var clonedChildren = Children.Select(c => c.Clone());
            return new RootNode(clonedChildren);
        }
    }
}