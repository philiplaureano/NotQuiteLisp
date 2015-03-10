namespace ANTLR4.ParserHelpers
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    public interface ITreeBuilder
    {
        IParseTree CreateTree(ICharStream charStream);
    }
}