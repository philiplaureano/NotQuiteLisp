using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    using NotQuiteLisp.AST.Interfaces;

    public interface IFunction : ISymbol
    {
        INode<AstNode> Eval(INode<AstNode> node);
    }
}