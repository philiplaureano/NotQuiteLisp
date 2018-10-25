namespace NotQuiteLisp.Core
{
    using NotQuiteLisp.AST.Interfaces;

    public class TreeMapper<TItem>
    {
        public TreeMap<TItem> CreateMap(INode<TItem> rootNode)
        {
            var map = new TreeMap<TItem>();
            BuildMap(rootNode, map);
            return map;
        }

        private void BuildMap(INode<TItem> parentNode, TreeMap<TItem> map)
        {
            foreach (var child in parentNode.Children)
            {
                BuildMap(child, map);
                map.SetParentFor(child, parentNode);
            }
        }
    }
}