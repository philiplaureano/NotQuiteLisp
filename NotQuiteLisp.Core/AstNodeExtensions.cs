using System.Collections.Generic;
using System.Security.Cryptography;
using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    using System;

    using NotQuiteLisp.AST.Interfaces;

    public static class AstNodeExtensions
    {
        public static IEnumerable<INode<T>> Descendants<T>(this INode<T> node, int? maxDepth = null)
        {
            var descendants = new List<INode<T>>();
            node.Descend((currentDepth, currentNode)=> descendants.Add(currentNode));

            return descendants;
        }

        public static void Descend<T>(this INode<T> node,
            Action<int, INode<T>> callbackFunc,
            int? maxDepth = null)
        {
            node.Descend(callbackFunc, 0, maxDepth);
        }

        private static void Descend<T>(
            this INode<T> node,
            Action<int, INode<T>> callbackFunc,
            int currentDepth,
            int? maxDepth = null)
        {
            foreach (var child in node.Children)
            {
                child.Descend(callbackFunc, currentDepth + 1, maxDepth);

                if (maxDepth == null || (currentDepth < maxDepth.Value))
                    callbackFunc(currentDepth, child);
            }
        }
    }
}