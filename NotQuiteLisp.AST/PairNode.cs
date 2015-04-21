using System.Linq;

namespace NotQuiteLisp.AST
{
    public class PairNode : AstNode
    {
        public PairNode(AstNode key, AstNode valueNode)
            : base(new[] { key, valueNode })
        {
        }

        public override AstNode Clone()
        {
            var children = Children.ToArray();
            return new PairNode(children[0], children[1]);
        }
    }
}