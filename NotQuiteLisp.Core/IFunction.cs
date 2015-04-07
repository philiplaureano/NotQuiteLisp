using NotQuiteLisp.AST;

namespace NotQuiteLisp.Core
{
    public interface IFunction
    {
        AstNode Eval(AstNode node);
    }
}