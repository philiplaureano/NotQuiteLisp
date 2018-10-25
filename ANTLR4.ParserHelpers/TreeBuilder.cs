namespace ANTLR4.ParserHelpers
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    public class TreeBuilder : ITreeBuilder
    {
        private readonly ILanguageStrategy _strategy;

        public TreeBuilder(ILanguageStrategy strategy)
        {
            _strategy = strategy;
        }

        public IParseTree CreateTree(ICharStream charStream)
        {
            var lexer = _strategy.CreateTokenSource(charStream);
            var tokenStream = _strategy.CreateTokenStream(lexer);

            return _strategy.CreateParseTree(tokenStream);
        }
    }
}