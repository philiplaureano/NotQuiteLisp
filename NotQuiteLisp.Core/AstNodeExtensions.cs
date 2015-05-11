using System.Collections.Generic;
using System.Security.Cryptography;
using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    using System;
    using System.Linq;

    using NotQuiteLisp.AST.Interfaces;

    public static class AstNodeExtensions
    {
        public static IEnumerable<INode<T>> Descendants<T>(this INode<T> node, int? maxDepth = null)
        {
            var descendants = new List<INode<T>>();
            node.Descend((currentDepth, childIndex, currentNode) => descendants.Add(currentNode));

            return descendants;
        }

        public static void Descend<T>(this INode<T> node, Action<int, int, INode<T>> callbackFunc, int? maxDepth = null)
        {
            node.Descend(callbackFunc, 0, maxDepth);
        }

        public static int Height<T>(this INode<T> node)
        {
            var depths = new List<int>();
            node.Descend((currentDepth, childIndex, currentNode) => depths.Add(currentDepth), 1);

            return depths.OrderByDescending(d => d).FirstOrDefault();
        }

        private static void Descend<T>(
            this INode<T> node,
            Action<int, int, INode<T>> callbackFunc,
            int currentDepth,
            int? maxDepth = null)
        {
            var childIndex = 0;
            foreach (var child in node.Children)
            {
                child.Descend(callbackFunc, currentDepth + 1, maxDepth);

                if (maxDepth == null || (currentDepth < maxDepth.Value))
                    callbackFunc(currentDepth, childIndex, child);

                childIndex++;
            }
        }
    }
}