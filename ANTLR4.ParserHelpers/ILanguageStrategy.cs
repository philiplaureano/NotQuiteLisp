namespace ANTLR4.ParserHelpers
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    public interface ILanguageStrategy
    {
        ITokenSource CreateTokenSource(ICharStream charStream);
        ITokenStream CreateTokenStream(ITokenSource tokenSource);
        IParseTree CreateParseTree(ITokenStream tokenStream);
    }
}