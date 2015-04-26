using NotQuiteLisp.AST;

namespace NotQuiteLisp.Visitors
{
    using NotQuiteLisp.AST.Interfaces;

    public class AstVisitor<TResult> : IVisitor<INode<AstNode>, TResult>
    {
        public virtual TResult Visit(INode<AstNode> subject)
        {
            return Visit(subject, true);
        }

        protected TResult Visit(INode<AstNode> subject, bool throwOnError)
        {
            return subject.Accept<TResult>(this, throwOnError);
        }
    }
}