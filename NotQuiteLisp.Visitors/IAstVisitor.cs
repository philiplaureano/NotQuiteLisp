using NotQuiteLisp.AST;

namespace NotQuiteLisp.Visitors
{
    public interface IAstVisitor<out TResult> : IVisitor<AstNode, TResult>
    {
    }
}