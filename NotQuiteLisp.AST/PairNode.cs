using System.Linq;

namespace NotQuiteLisp.AST
{
    using NotQuiteLisp.AST.Interfaces;

    public class PairNode : AstNode
    {
        public PairNode(INode<AstNode> key, INode<AstNode> valueNode)
            : base(new[] { key, valueNode })
        {
        }

        public override INode<AstNode> Clone()
        {
            var children = Children.ToArray();
            return new PairNode(children[0], children[1]);
        }
    }
}