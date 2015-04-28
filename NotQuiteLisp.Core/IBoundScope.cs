using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    using NotQuiteLisp.AST.Interfaces;

    public interface IBoundScope : INode<IScope>, INamedScope
    {
        INode<AstNode> Node { get; }
        IScope TargetScope { get; }
    }
}