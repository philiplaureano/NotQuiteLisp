namespace NotQuiteLisp.Parser
{
    using ANTLR4.ParserHelpers;

    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    public class NqlLanguage : ILanguageStrategy
    {
        public ITokenSource CreateTokenSource(ICharStream charStream)
        {
            return new NQLLexer(charStream);
        }

        public ITokenStream CreateTokenStream(ITokenSource tokenSource)
        {
            return new CommonTokenStream(tokenSource);
        }

        public IParseTree CreateParseTree(ITokenStream tokenStream)
        {
            var parser = new NQLParser(tokenStream);

            var parseTree = parser.compileUnit();

            return parseTree;
        }
    }
}
