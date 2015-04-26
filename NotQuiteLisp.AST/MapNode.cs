using System.Linq.Expressions;

namespace NotQuiteLisp.AST
{
    using System.Collections.Generic;
    using System.Linq;

    using NotQuiteLisp.AST.Interfaces;

    public class MapNode : ElementNode
    {
        public MapNode(IEnumerable<KeyValuePairNode> entries) : base(entries)
        {
        }

        public IEnumerable<KeyValuePairNode> Entries
        {
            get
            {
                return Children.Select(child => child as KeyValuePairNode)
                    .Where(child => child != null);
            }
        }

        public override INode<AstNode> Clone()
        {
            var clonedEntries = Entries.Select(kvp => (KeyValuePairNode)kvp.Clone());
            return new MapNode(clonedEntries);
        }
    }
}