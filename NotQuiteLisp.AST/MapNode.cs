using System.Linq.Expressions;

namespace NotQuiteLisp.AST
{
    using System.Collections.Generic;
    using System.Linq;

    using NotQuiteLisp.AST.Interfaces;

    public class MapNode : ElementNode
    {
        public MapNode(IEnumerable<PairNode> entries) : base(entries)
        {
        }

        public IEnumerable<PairNode> Entries
        {
            get
            {
                return Children.Select(child => child as PairNode)
                    .Where(child => child != null);
            }
        }

        public override INode<AstNode> Clone()
        {
            var clonedEntries = Entries.Select(kvp => (PairNode)kvp.Clone());
            return new MapNode(clonedEntries);
        }
    }
}