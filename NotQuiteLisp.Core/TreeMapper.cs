using System;
using System.Collections.Generic;
using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    public class TreeMapper
    {
        public TreeMap CreateMap(AstNode rootNode)
        {
            var map = new TreeMap();
            BuildMap(rootNode, map);
            return map;
        }

        private void BuildMap(AstNode parentNode, TreeMap map)
        {
            foreach (var child in parentNode.Children)
            {
                BuildMap(child, map);
                map.SetParentFor(child, parentNode);
            }
        }
    }
}