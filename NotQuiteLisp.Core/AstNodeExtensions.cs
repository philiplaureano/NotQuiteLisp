using System.Collections.Generic;
using System.Security.Cryptography;
using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    using NotQuiteLisp.AST.Interfaces;

    public static class AstNodeExtensions
    {
        public static IEnumerable<INode<T>> Descendants<T>(this INode<T> node, int? maxDepth = null)
        {
            var descendants = new List<INode<T>>();
            AddDescendants(node, descendants, 0, maxDepth);

            return descendants;
        }

        private static void AddDescendants<T>(this INode<T> node, ICollection<INode<T>> descendants, int currentDepth, int? maxDepth = null)
        {
            foreach (var child in node.Children)
            {
                AddDescendants(child, descendants, currentDepth + 1, maxDepth);

                if (maxDepth == null || (currentDepth < maxDepth.Value))
                    descendants.Add(child);
            }
        }
    }
}