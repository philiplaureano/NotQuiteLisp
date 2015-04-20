using NotQuiteLisp.AST;

namespace NotQuiteLisp.Visitors
{
    public class AstVisitor<TResult> : IVisitor<AstNode, TResult>
    {
        public TResult Visit(AstNode subject)
        {
            return subject.Accept<TResult>(this);
        }
    }
}