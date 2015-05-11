using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    using NotQuiteLisp.AST.Interfaces;

    public interface IBoundScope<TItem> : INode<IScope<TItem>>, INamedScope<TItem>
        where TItem : ISymbol
    {
        INode<AstNode> Node { get; }
        IScope<TItem> TargetScope { get; }
    }
}