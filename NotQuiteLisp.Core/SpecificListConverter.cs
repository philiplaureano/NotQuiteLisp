namespace NotQuiteLisp.Core
{
    using System;
    using System.Collections.Concurrent;
    using AST;

    using NotQuiteLisp.AST.Interfaces;

    public abstract class SpecificListConverter : ListConverter
    {
        private readonly ConcurrentBag<Guid> _declarations = new ConcurrentBag<Guid>();
        private readonly int _expectedParameterCount;

        private readonly string _targetSymbol;

        protected SpecificListConverter(int expectedParameterCount, string targetSymbol)
        {
            if (expectedParameterCount < 0)
                throw new ArgumentOutOfRangeException("expectedParameterCount");

            if (string.IsNullOrEmpty(targetSymbol))
                throw new ArgumentException("The target symbol cannot be a null or empty string.", "targetSymbol");

            _expectedParameterCount = expectedParameterCount;
            _targetSymbol = targetSymbol;
        }

        protected override bool HasValidChildNodes(INode<AstNode> parent, INode<AstNode>[] children)
        {
            return children.Length == _expectedParameterCount + 1;
        }

        protected override bool ShouldBeConverted(SymbolNode symbolNode)
        {
            return symbolNode != null && symbolNode.Symbol == _targetSymbol;
        }        
    }
}