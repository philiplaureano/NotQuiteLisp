namespace ANTLR4.ParserHelpers
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    public class TreeBuilder : ITreeBuilder
    {
        private readonly ILanguageStrategy _strategy;

        public TreeBuilder(ILanguageStrategy strategy)
        {
            this._strategy = strategy;
        }

        public IParseTree CreateTree(ICharStream charStream)
        {
            var lexer = this._strategy.CreateTokenSource(charStream);
            var tokenStream = this._strategy.CreateTokenStream(lexer);

            return this._strategy.CreateParseTree(tokenStream);
        }
    }
}