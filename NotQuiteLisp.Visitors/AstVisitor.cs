using NotQuiteLisp.AST;

namespace NotQuiteLisp.Visitors
{
    public class AstVisitor<TResult> : IVisitor<AstNode, TResult>
    {
        public virtual TResult Visit(AstNode subject)
        {
            return Visit(subject, true);
        }

        protected TResult Visit(AstNode subject, bool throwOnError)
        {
            return subject.Accept<TResult>(this, throwOnError);
        }
    }
}